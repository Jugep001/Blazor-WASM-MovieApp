using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using Lucene.Net.Search.Highlight;
using Microsoft.AspNetCore.Components;
using Blazor_WASM_MovieApp.Models;

namespace Blazor_WASM_MovieApp.Client.Repositories
{
    public class WASM_MovieRepository
    {
        const LuceneVersion luceneVersion = LuceneVersion.LUCENE_48;
        Lucene.Net.Analysis.Analyzer standardAnalyzer = new StandardAnalyzer(luceneVersion);
        Lucene.Net.Analysis.Analyzer classicAnalyzer = new ClassicAnalyzer(luceneVersion);


        public List<MarkupString> HighlightDescription(string description, string searchString)
        {
            const LuceneVersion luceneVersion = LuceneVersion.LUCENE_48;
            Lucene.Net.Analysis.Analyzer standardAnalyzer = new StandardAnalyzer(luceneVersion);


            //Open the Directory using a Lucene Directory class
            string indexName = "Description_index";
            string indexPath = Path.Combine(Environment.CurrentDirectory, indexName);

            using Lucene.Net.Store.Directory indexDir = FSDirectory.Open(indexPath);

            IndexWriterConfig indexConfig = new IndexWriterConfig(luceneVersion, standardAnalyzer);
            indexConfig.OpenMode = OpenMode.CREATE;                             // create/overwrite index
            IndexWriter writer = new IndexWriter(indexDir, indexConfig);

            Document doc = new Document();
            doc.Add(new TextField("Description", description, Field.Store.YES));
            writer.AddDocument(doc);

            writer.Commit();

            using IndexReader reader = DirectoryReader.Open(indexDir);
            IndexSearcher searcher = new IndexSearcher(reader);


            QueryParser parser = new QueryParser(luceneVersion, "Description", classicAnalyzer);
            Query query = parser.Parse(searchString);
            TopDocs topDocs = searcher.Search(query, n: 10);

            List<Movie> movies = new List<Movie>();

            QueryScorer scorer = new QueryScorer(query);

            SimpleHTMLFormatter formatter = new SimpleHTMLFormatter("<span style='color:maroon; font-weight:bold;'>", "</span>");
            Highlighter highlighter = new Highlighter(formatter, scorer);
            List<MarkupString> descriptionList = new List<MarkupString>();
            string[] fragment = null;
            for (int i = 0; i < topDocs.TotalHits; i++)
            {
                int docId = topDocs.ScoreDocs[i].Doc;
                Document resultDoc = searcher.Doc(topDocs.ScoreDocs[i].Doc);

                string text = resultDoc.Get("Description");

                TokenStream stream = TokenSources.GetAnyTokenStream(reader, docId, "Description", resultDoc, standardAnalyzer);
                IFragmenter fragmenter = new SimpleSpanFragmenter(scorer);
                highlighter.TextFragmenter = fragmenter;
                fragment = highlighter.GetBestFragments(stream, text, 2);


            }
            if (fragment != null)
            {
                foreach (var highlight in fragment)
                {
                    descriptionList.Add((MarkupString)highlight);
                }
            }

            writer.Dispose();
            reader.Dispose();

            return descriptionList;
        }
    }
}
