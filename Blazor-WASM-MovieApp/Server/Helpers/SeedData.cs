using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Store;
using Lucene.Net.Util;
using LuceneDirectory = Lucene.Net.Store.Directory;
using System.Drawing;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Blazor_WASM_MovieApp.Data;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace Blazor_WASM_MovieApp.Models
{
    public class SeedData
    {
        

        public static async void Initialize(IServiceProvider serviceProvider, IWebHostEnvironment env)
        {
            using (var context = new BlazorMovieContext(serviceProvider.GetRequiredService<DbContextOptions<BlazorMovieContext>>()))
            {
                string[] roles = new string[] { "admin", "reader", "writer" };


                var roleStore = new RoleStore<IdentityRole>(context);
                foreach (string role in roles)
                {


                    if (!context.Roles.Any(r => r.Name == role))
                    {
                        await roleStore.CreateAsync(new IdentityRole
                        {
                            Name = role,
                            NormalizedName = role
                        });
                    }

                }




                var admin = new IdentityUser
                {
                    Email = "xxxx@example.com",
                    NormalizedEmail = "XXXX@EXAMPLE.COM",
                    UserName = "Admin",
                    NormalizedUserName = "ADMIN",
                    PhoneNumber = "+111111111111",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D")
                };

                var reader = new IdentityUser
                {
                    Email = "xxxx@example.com",
                    NormalizedEmail = "XXXX@EXAMPLE.COM",
                    UserName = "Reader",
                    NormalizedUserName = "READER",
                    PhoneNumber = "+111111111111",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D")
                };

                var writer = new IdentityUser
                {
                    Email = "xxxx@example.com",
                    NormalizedEmail = "XXXX@EXAMPLE.COM",
                    UserName = "Writer",
                    NormalizedUserName = "WRITER",
                    PhoneNumber = "+111111111111",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D")
                };


                if (!context.Users.Any(u => u.UserName == admin.UserName))
                {
                    var passwordhasher = new PasswordHasher<IdentityUser>();
                    var hashed = passwordhasher.HashPassword(admin, "secret");
                    admin.PasswordHash = hashed;

                    var hashed2 = passwordhasher.HashPassword(reader, "secret");
                    reader.PasswordHash = hashed2;

                    var hashed3 = passwordhasher.HashPassword(writer, "secret");
                    writer.PasswordHash = hashed3;

                    var userStore = new UserStore<IdentityUser>(context);
                    var result = userStore.CreateAsync(admin);
                    var result2 = userStore.CreateAsync(reader);
                    var result3 = userStore.CreateAsync(writer);

                    await userStore.AddToRoleAsync(admin, "admin");
                    await userStore.AddToRoleAsync(reader, "reader");
                    await userStore.AddToRoleAsync(writer, "writer");

                }

                context.SaveChanges();





                if (context.Movies.Any())
                {
                    return;
                }

                if (!context.Movies.Any() && !context.Genres.Any() && !context.Functions.Any() && !context.People.Any())
                {
                    context.Functions.AddRange(
                        new Function
                        {
                            FunctionName = "Schnitt",
                            Credits = new List<Credit>(),
                            IsRoleRequired = false

                        },
                        new Function
                        {
                            FunctionName = "Regie",
                            Credits = new List<Credit>(),
                            IsRoleRequired = false

                        },
                        new Function
                        {
                            FunctionName = "Buch",
                            Credits = new List<Credit>(),
                            IsRoleRequired = false

                        },
                        new Function
                        {
                            FunctionName = "Kamera",
                            Credits = new List<Credit>(),
                            IsRoleRequired = false

                        },
                        new Function
                        {
                            FunctionName = "Darsteller",
                            Credits = new List<Credit>(),
                            IsRoleRequired = true

                        }
                    );

                    context.SaveChanges();


                    context.People.AddRange(
                        new Person
                        {
                            Vorname = "George",
                            Name = "Lucas"
                            
                        },
                        new Person
                        {
                            Vorname = "Michael",
                            Name = "Bay"
                        },
                        new Person
                        {
                            Vorname = "Tom",
                            Name = "Holland"
                        },
                        new Person
                        {
                            Vorname = "Tom",
                            Name = "Cruise"
                        }

                        );

                    context.SaveChanges();


                    Function Schnitt = context.Functions.Where(func => func.FunctionName == "Schnitt").First();
                    Function Regie = context.Functions.Where(func => func.FunctionName == "Regie").First();
                    Function Buch = context.Functions.Where(func => func.FunctionName == "Buch").First();

                    Person Lucas = context.People.Where(p => p.Name == "Lucas").First();
                    Person Bay = context.People.Where(p => p.Name == "Bay").First();
                    Person Cruise = context.People.Where(p => p.Name == "Cruise").First();

                    context.Genres.AddRange(
                        new Genre
                        {
                            Name = "Western",
                            Movies = new List<Movie>()
                        },
                        new Genre
                        {
                            Name = "Comedy",
                            Movies = new List<Movie>()
                        },
                        new Genre
                        {
                            Name = "Action",
                            Movies = new List<Movie>()
                        }


                        );


                    context.SaveChanges();

                    Genre WesternGenre = context.Genres.Where(genre => genre.Name == "Western").First();
                    Genre ComedyGenre = context.Genres.Where(genre => genre.Name == "Comedy").First();
                    Genre ActionGenre = context.Genres.Where(genre => genre.Name == "Action").First();


                    context.Movies.AddRange(


                            new Movie
                            {
                                Title = "When Harry Met Sally",
                                ReleaseDate = DateTime.ParseExact("12/02/1989", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                                Rating = "R",
                                Genres = new List<Genre> { ComedyGenre },
                                Price = 7.99M,
                                Description = "Im Verlauf von zwölf Jahren begegnen sich in größeren Abständen ein Mann und eine Frau in New York, wobei sich ihre Gespräche und Dispute um Freundschaft, " +
                                              "Liebe und Sex allmählich doch als tragfähige Basis für eine Ehe erweisen. Eine von hervorragenden Darstellern, pointierten Dialogen und einer behutsam-zurückhaltenden " +
                                              "Inszenierung geprägte Komödie, die mit fröhlichem Witz und viel Humor einen ebenso amüsanten wie hintergründig-besinnlichen Kosmos menschlichen Miteinanders entwirft. - Sehenswert ab 16.",
                                ShortDescription = "Im Verlauf von zwölf Jahren begegnen sich in größeren Abständen...",
                                Credits = new List<Credit>
                                {
                            new Credit()
                            {
                                Function = Schnitt,
                                Person = Lucas,
                                Position = 0
                            },
                            new Credit()
                            {
                                Function = Buch,
                                Person = Bay,
                                Position = 1
                            },
                            new Credit()
                            {
                                Function = Regie,
                                Person = Cruise,
                                Position = 2
                            }

                                }
                            },
                            new Movie
                            {
                                Title = "Ghostbusters",
                                ReleaseDate = DateTime.ParseExact("13/03/1984", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                                Rating = "PG",
                                Genres = new List<Genre> { ActionGenre },
                                Price = 8.99M,
                                Description = "Drei Pseudo-Wissenschaftler bekämpfen erfolgreich sich manifestierende Geister aus dem Sumerer-Reich. " +
                                              "Die erstaunliche Perfektion der Spezial-Effekte steht in umgekehrtem Verhältnis zur Intelligenz der Story und Dramaturgie. " +
                                              "Eine sich verselbständigende Trickschau, deren beabsichtigte Komik nur schwach zu ahnen ist; immerhin unterhaltsam, wenn auch durch die deutsche Synchronisation zusätzlich belastet. - Ab 12.",
                                ShortDescription = "Drei Pseudo-Wissenschaftler bekämpfen erfolgreich sich manifestierende Geister aus dem Sumerer-Reich....",
                                Credits = new List<Credit>
                                {
                            new Credit()
                            {
                                Function = Schnitt,
                                Person = Lucas,
                                Position = 0
                            },
                            new Credit()
                            {
                                Function = Buch,
                                Person = Bay,
                                Position = 1
                            },
                            new Credit()
                            {
                                Function = Regie,
                                Person = Cruise,
                                Position = 2
                            }

                                }
                            },

                            new Movie
                            {
                                Title = "Ghostbusters 2",
                                ReleaseDate = DateTime.ParseExact("23/02/1986", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                                Rating = "PG",
                                Genres = new List<Genre> { ActionGenre },
                                Price = 9.99M,
                                Description = "Die drei geisterjagenden Pseudowissenschaftler, die bereits 1984 in \"Ghostbusters - Die Geisterjäger\" erfolgreich Geister aus dem Sumerer-Reich bekämpften" +
                                              ", sind dieses Mal im Einsatz gegen einen mittelalterlichen Ritter, der es leid ist, in ein Gemälde gebannt zu sein, " +
                                              "sowie gegen Unmengen bösartigen Schleims. Eine ideenarme, humorlose Fortsetzung der beliebten Anti-Geister-Geschichte. - Ab 14.",
                                ShortDescription = "Die drei geisterjagenden Pseudowissenschaftler, die bereits 1984 in \"Ghostbusters - Die Geisterjäger\"...",
                                Credits = new List<Credit>
                                {
                            new Credit()
                            {
                                Function = Schnitt,
                                Person = Lucas,
                                Position = 0
                            },
                            new Credit()
                            {
                                Function = Buch,
                                Person = Bay,
                                Position = 1
                            },
                            new Credit()
                            {
                                Function = Regie,
                                Person = Cruise,
                                Position = 2
                            }

                                }
                            },

                            new Movie
                            {
                                Title = "Rio Bravo",
                                ReleaseDate = DateTime.ParseExact("15/04/1959", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                                Rating = "R",
                                Genres = new List<Genre> { WesternGenre },
                                Price = 3.99M,
                                Description = "Sheriff John Chance gerät bei der Festnahme eines Mörders in Bedrängnis: Der Bruder des Täters, Boß einer Bande von Revolvermännern, belagert das Gefängnis. " +
                                              "Nur durch die Mithilfe eines versoffenen Hilfssheriffs, eines kauzigen Alten und eines jungen Scharfschützen kann die Lage bereinigt werden. " +
                                              "Herausragender Western von Howard Hawks, der die einfache, aber spannende Story mit professioneller Gelassenheit und ironischen Zwischentönen inszeniert. " +
                                              "Die Figurenzeichnung ist brillant. Das Thema wurde vom selben Regisseur später in den Filmen \"El Dorado\" (1966) und \"Rio Lobo\" (1970) variiert. - Ab 14.",
                                ShortDescription = "Sheriff John Chance gerät bei der Festnahme eines Mörders in Bedrängnis...",
                                Credits = new List<Credit>
                                {
                            new Credit()
                            {
                                Function = Schnitt,
                                Person = Lucas,
                                Position = 0
                            },
                            new Credit()
                            {
                                Function = Buch,
                                Person = Bay,
                                Position = 1
                            },
                            new Credit()
                            {
                                Function = Regie,
                                Person = Cruise,
                                Position = 2
                            }

                                }
                            }

                            );

                    context.SaveChanges();

                    //AddMoviesToDatabase(context, 0, 1000);
                    //AddMoviesToDatabase(context, 1000, 2000);
                    //AddMoviesToDatabase(context, 2000, 3000);
                    //AddMoviesToDatabase(context, 3000, 4000);



                    Movie ghostbusters = context.Movies.Where(movie => movie.Title == "Ghostbusters").First();
                    Movie ghostbusters2 = context.Movies.Where(movie => movie.Title == "Ghostbusters 2").First();
                    Movie When_Harry_Met_Sally = context.Movies.Where(movie => movie.Title == "When Harry Met Sally").First();
                    Movie Rio_Bravo = context.Movies.Where(movie => movie.Title == "Rio Bravo").First();

                    context.Logs.AddRange(
                        new Changelog
                        {
                            CreatedAt = DateTime.Now,
                            CreatedBy = "Admin",
                            UpdatedAt = DateTime.Now,
                            UpdatedBy = "Admin",
                            Movie = ghostbusters,
                            
                        },
                        new Changelog
                        {
                            CreatedAt = DateTime.Now,
                            CreatedBy = "Admin",
                            UpdatedAt = DateTime.Now,
                            UpdatedBy = "Admin",
                            Movie = ghostbusters2,

                        },
                        new Changelog
                        {
                            CreatedAt = DateTime.Now,
                            CreatedBy = "Admin",
                            UpdatedAt = DateTime.Now,
                            UpdatedBy = "Admin",
                            Movie = When_Harry_Met_Sally,

                        },
                        new Changelog
                        {
                            CreatedAt = DateTime.Now,
                            CreatedBy = "Admin",
                            UpdatedAt = DateTime.Now,
                            UpdatedBy = "Admin",
                            Movie = Rio_Bravo,

                        }
                        );
                    context.SaveChanges();

                    AddImageToDatabase(context, "Ghostbusters1.jpg", ghostbusters, env);
                    AddImageToDatabase(context, "Ghostbusters2.jpg", ghostbusters2, env);
                    AddImageToDatabase(context, "When Harry Met Sally.jpg", When_Harry_Met_Sally, env);
                    AddImageToDatabase(context, "rio.jpg", Rio_Bravo, env);

                    const LuceneVersion luceneVersion = LuceneVersion.LUCENE_48;
                    Lucene.Net.Analysis.Analyzer standardAnalyzer = new StandardAnalyzer(luceneVersion);
                    string indexName = "Movies_index";
                    string indexPath = Path.Combine(Environment.CurrentDirectory, indexName);
                    using LuceneDirectory indexDir = FSDirectory.Open(indexPath);
                    IndexWriterConfig indexConfig = new IndexWriterConfig(luceneVersion, standardAnalyzer);
                    IndexWriter iWriter;

                    indexConfig.OpenMode = OpenMode.CREATE;
                    iWriter = new IndexWriter(indexDir, indexConfig);

                    foreach (Movie movie in context.Movies.Include(movie => movie.Genres))
                    {
                        Document doc = new Document();
                        doc.Add(new StringField("Id", movie.Id.ToString(), Field.Store.YES));
                        doc.Add(new TextField("Title", movie.Title, Field.Store.YES));
                        doc.Add(new TextField("Description", movie.Description, Field.Store.YES));
                        if (movie.Genres != null)
                        {
                            foreach (Genre genre in movie.Genres)
                            {
                                doc.Add(new TextField("Genre", genre.Name, Field.Store.YES));
                            }
                        }
                        iWriter.AddDocument(doc);
                    }
                    iWriter.Commit();
                    iWriter.Dispose();

                }
            }
        }
        public async static void AddImageToDatabase(BlazorMovieContext context, string imgName, Movie movie, IWebHostEnvironment env)
        {
            System.Drawing.Image image = System.Drawing.Image.FromFile($"C:\\Users\\Jugep\\Desktop\\Filmbilder\\{imgName}");
            long size = new System.IO.FileInfo($"C:\\Users\\Jugep\\Desktop\\Filmbilder\\{imgName}").Length;
            Bitmap bm = ResizeImage(image, 300, 300);
            Bitmap bmThumb = ResizeImage(image, 109, 150);


            string path = $"{env.WebRootPath}\\images\\{imgName}";
            string thumbnailPath;
            Guid imageId = Guid.NewGuid();

            string imageName = Path.GetFileNameWithoutExtension(path);
            string imageExtension = Path.GetExtension(path);
            path = $"{env.WebRootPath}\\images\\{imageName + imageId.ToString() + imageExtension}";
            thumbnailPath = $"{env.WebRootPath}\\images\\thumbnails\\{imageName + imageId.ToString() + imageExtension}";

            Image imageModel = new Image
            {
                ImageName = imageName + imageId.ToString() + imageExtension,
                Path = path,
                ThumbnailPath = thumbnailPath,
                Size = size,
                MovieId = movie.Id,
                Movie = movie
            };

            context.Add(imageModel);

            using (FileStream fs = File.Create(path))
            {
                using (MemoryStream ms = new MemoryStream())
                {

                    bm.Save(ms, ImageFormat.Jpeg);
                    ms.Position = 0;
                    await ms.CopyToAsync(fs);
                }

            }
            using (FileStream fs = File.Create(thumbnailPath))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    bmThumb.Save(ms, ImageFormat.Jpeg);
                    ms.Position = 0;
                    await ms.CopyToAsync(fs);

                }

            }

            context.SaveChanges();

        }

        public static Bitmap ResizeImage(System.Drawing.Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public static void AddMoviesToDatabase(BlazorMovieContext context, int start, int end)
        {
            Genre WesternGenre = context.Genres.Where(genre => genre.Name == "Western").First();
            Genre ComedyGenre = context.Genres.Where(genre => genre.Name == "Comedy").First();
            Genre ActionGenre = context.Genres.Where(genre => genre.Name == "Action").First();

            Function Schnitt = context.Functions.Where(func => func.FunctionName == "Schnitt").First();
            Function Regie = context.Functions.Where(func => func.FunctionName == "Regie").First();
            Function Buch = context.Functions.Where(func => func.FunctionName == "Buch").First();

            Person Lucas = context.People.Where(p => p.Name == "Lucas").First();
            Person Bay = context.People.Where(p => p.Name == "Bay").First();
            Person Cruise = context.People.Where(p => p.Name == "Cruise").First();

            for (int i = start; i < end; i++)
            {

                context.Movies.AddRange(


                            new Movie
                            {
                                Title = "When Harry Met Sally" + i,
                                ReleaseDate = DateTime.ParseExact("12/02/1989", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                                Rating = "R",
                                Genres = new List<Genre> { ComedyGenre },
                                Price = 7.99M,
                                Description = "Im Verlauf von zwölf Jahren begegnen sich in größeren Abständen ein Mann und eine Frau in New York, wobei sich ihre Gespräche und Dispute um Freundschaft, " +
                                              "Liebe und Sex allmählich doch als tragfähige Basis für eine Ehe erweisen. Eine von hervorragenden Darstellern, pointierten Dialogen und einer behutsam-zurückhaltenden " +
                                              "Inszenierung geprägte Komödie, die mit fröhlichem Witz und viel Humor einen ebenso amüsanten wie hintergründig-besinnlichen Kosmos menschlichen Miteinanders entwirft. - Sehenswert ab 16.",
                                Credits = new List<Credit>
                                {
                            new Credit()
                            {
                                Function = Schnitt,
                                Person = Lucas
                            },
                            new Credit()
                            {
                                Function = Buch,
                                Person = Bay
                            },
                            new Credit()
                            {
                                Function = Regie,
                                Person = Cruise
                            }

                                }
                            },
                            new Movie
                            {
                                Title = "Ghostbusters" + i,
                                ReleaseDate = DateTime.ParseExact("13/03/1984", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                                Rating = "PG",
                                Genres = new List<Genre> { ActionGenre },
                                Price = 8.99M,
                                Description = "Drei Pseudo-Wissenschaftler bekämpfen erfolgreich sich manifestierende Geister aus dem Sumerer-Reich. " +
                                              "Die erstaunliche Perfektion der Spezial-Effekte steht in umgekehrtem Verhältnis zur Intelligenz der Story und Dramaturgie. " +
                                              "Eine sich verselbständigende Trickschau, deren beabsichtigte Komik nur schwach zu ahnen ist; immerhin unterhaltsam, wenn auch durch die deutsche Synchronisation zusätzlich belastet. - Ab 12.",
                                Credits = new List<Credit>
                                {
                            new Credit()
                            {
                                Function = Schnitt,
                                Person = Lucas
                            },
                            new Credit()
                            {
                                Function = Buch,
                                Person = Bay
                            },
                            new Credit()
                            {
                                Function = Regie,
                                Person = Cruise
                            }

                                }
                            },

                            new Movie
                            {
                                Title = "Rio Bravo" + i,
                                ReleaseDate = DateTime.ParseExact("15/04/1959", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                                Rating = "R",
                                Genres = new List<Genre> { WesternGenre },
                                Price = 3.99M,
                                Description = "Sheriff John Chance gerät bei der Festnahme eines Mörders in Bedrängnis: Der Bruder des Täters, Boß einer Bande von Revolvermännern, belagert das Gefängnis. " +
                                              "Nur durch die Mithilfe eines versoffenen Hilfssheriffs, eines kauzigen Alten und eines jungen Scharfschützen kann die Lage bereinigt werden. " +
                                              "Herausragender Western von Howard Hawks, der die einfache, aber spannende Story mit professioneller Gelassenheit und ironischen Zwischentönen inszeniert. " +
                                              "Die Figurenzeichnung ist brillant. Das Thema wurde vom selben Regisseur später in den Filmen \"El Dorado\" (1966) und \"Rio Lobo\" (1970) variiert. - Ab 14.",
                                Credits = new List<Credit>
                                {
                            new Credit()
                            {
                                Function = Schnitt,
                                Person = Lucas
                            },
                            new Credit()
                            {
                                Function = Buch,
                                Person = Bay
                            },
                            new Credit()
                            {
                                Function = Regie,
                                Person = Cruise
                            }

                                }
                            }

                            );

            }

            context.SaveChanges();

        }
    }
}




