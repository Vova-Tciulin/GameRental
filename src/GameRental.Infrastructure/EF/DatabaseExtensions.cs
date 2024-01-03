using GameRental.Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Console = GameRental.Domain.Entities.Console;

namespace GameRental.Infrastructure.EF;

/// <summary>
/// Применяет созданные миграции при запуске проекта,
/// а также если бд пуста, заполняет ее начальными данными
/// </summary>

public class DatabaseExtensions
{
    public static async Task MigrateDatabase(WebApplication app)
    {
        using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var context = scope.ServiceProvider.GetService<GameRentalDbContext>();

            int maxAttempts = 3;
            int attemptCount = 0;
            int delay = 5;
            
            while (attemptCount < maxAttempts)
            {
                try
                {
                    Log.Information($"Attempt {attemptCount+1} to make a database migration");
                    context.Database.Migrate();
                    break;
                }
                catch (Exception e)
                {
                    Log.Error("A database migration error occurred");

                    attemptCount++;
                    Task.Delay(TimeSpan.FromSeconds(delay)).Wait();
                    delay += 5;
                }
            }
            
            await SeedData(context,scope);
        }
    }

    private static async Task SeedData(GameRentalDbContext db, IServiceScope scope)
    {
        var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

        var testUser = await userMgr.FindByEmailAsync("AliceSmith@email.com");
        if (testUser == null)
        {
            testUser = new User
            {
                Id = "dc06278d-a0fa-47db-b885-dc94490f381a",
                UserName = "AliceSmith@email.com",
                Email = "AliceSmith@email.com",
                LinkToNetwork = "@AliceLink",
                EmailConfirmed = true,
            };
            var result = userMgr.CreateAsync(testUser, "Pass123$").Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            await userMgr.AddToRoleAsync(testUser, "Admin");
            Log.Debug("alice created");
        }
        else
        {
            Log.Debug("alice already exists");
        }
        
        if (!db.ProductCategories.Any()&&
            !db.Products.Any()&&
            !db.Accounts.Any())
        {
            //категории
            
            var race = new ProductCategory()
            {
                Name = "Гонки",
                ImgPath = "299c6e4c-d6b2-4d06-81e0-ef202da1174e.jpeg"
            };
            var sport = new ProductCategory()
            {
                Name = "Спорт",
                ImgPath = "f422cfe4-b9b7-409b-9fc6-f0d3a82333c1.jpg"
            };
            var actions = new ProductCategory()
            {
                Name = "Боевики",
                ImgPath = "a1a2d79f-1e7d-4b31-9fa8-f8c7cee66b10.jpg"
            };
            var roleGames = new ProductCategory()
            {
                Name = "Ролевые игры",
                ImgPath = "86b8a952-1b32-4c0e-9af1-fcf35838420e.jpg"
            };
            await db.ProductCategories.AddRangeAsync(new[]
            {
                race,
                sport,
                actions,
                roleGames
            });

            //игры 
            var theWitcher = new Product()
            {
                Name = "Ведьмак 3: Дикая Охота",
                Translate = "Английский, Русский",
                Year = 2015,
                TransitTime = 40,
                ProductCategories = new List<ProductCategory>(new[] { roleGames, actions }),
                Description =
                    "<p><span style=\"color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\">Игра, получившая наибольшее число наград в 2015 году! Станьте охотником на чудовищ и отправляйтесь в эпическое путешествие, чтобы отыскать Дитя Предназначения: живое оружие невероятной разрушительной силы.</span><br style=\"margin: 0px; padding: 0px; color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\"><span style=\"color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\">Издание 'Игра года' включает оригинальную игру 'Ведьмак 3: Дикая Охота', два комплекта дополнений: 'Каменные Сердца' и 'Кровь и Вино', а также 16 различных наборов дополнительных материалов.</span><br style=\"margin: 0px; padding: 0px; color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\"></p>"
            };
            theWitcher.Images.Add(new Image()
            {
                ImgName = "55382614-71fa-4052-981b-df5dc7956d54.jpg",
                Product = theWitcher
            });
            
            var fifa = new Product()
            {
                Name = "FIFA 23",
                Translate = "Английский, Русский",
                Year = 2021,
                TransitTime = 15,
                ProductCategories = new List<ProductCategory>(new[] {sport }),
                Description =
                    "<p><span style=\"color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\">Всемирная игра</span><br style=\"margin: 0px; padding: 0px; color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\"><br style=\"margin: 0px; padding: 0px; color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\"><span style=\"color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\">В EA SPORTS\u2122 FIFA 23 всемирная игра становится еще лучше: технология HyperMotion2 обеспечивает еще большую реалистичность игрового процесса, после выхода в игре в виде обновлений будут представлены как мужской, так и женский FIFA World Cup\u2122, добавлены женские команды, кроссплатформенная игра* и многое другое. Оцените непревзойденную реалистичность — в FIFA 23 19 000+ игроков, 700+ команд, 100+ стадионов и 30+ лиг.</span><br style=\"margin: 0px; padding: 0px; color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\"><br style=\"margin: 0px; padding: 0px; color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\"><span style=\"color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\">Технология HyperMotion2</span><br style=\"margin: 0px; padding: 0px; color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\"><br style=\"margin: 0px; padding: 0px; color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\"><span style=\"color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\">Powered by Football\u2122, EA SPORTS\u2122 FIFA 23 впервые представляет технологию HyperMotion на ПК — благодаря двукратному увеличению количества записанных движений игровой процесс становится ещё реалистичнее. Технология HyperMotion2 открывает новые возможности в FIFA 23 и использует более 6000 реальных анимаций, созданных из миллионов кадров Передового захвата движений в матче 11 на 11. В результате FIFA 23 становится ещё ближе как к мужскому, так и к женскому футболу, а игроки более естественно перемещаются на поле, благодаря совершенно новой системе дриблинга, новой механике набора скорости и т.д.</span><br style=\"margin: 0px; padding: 0px; color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\"><br style=\"margin: 0px; padding: 0px; color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\"><span style=\"color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\">FIFA World Cup\u2122</span><br style=\"margin: 0px; padding: 0px; color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\"><br style=\"margin: 0px; padding: 0px; color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\"><span style=\"color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\">Играйте на международных турнирах высшего уровня — FIFA World Cup Qatar 2022\u2122 и FIFA Women’s World Cup Australia and New Zealand 2023\u2122 — в FIFA 23 вы сможете оценить весь накал страстей главного футбольного турнира в мире, который появится в игре после выхода и без дополнительной платы. А в FIFA 23 Ultimate Team\u2122 вас ждёт целый сезон контента, посвящённого FIFA World Cup Qatar 2022\u2122, с помощью которого вы сможете добавить своей команде колорита сборных.</span><br style=\"margin: 0px; padding: 0px; color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\"><br style=\"margin: 0px; padding: 0px; color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\"><span style=\"color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\">FIFA 23 Ultimate Team\u2122</span><br style=\"margin: 0px; padding: 0px; color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\"><br style=\"margin: 0px; padding: 0px; color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\"><span style=\"color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\">Соберите команду вашей мечты и играйте в самом популярном режиме FIFA — выбирайте игроков в состав из тысяч кандидатов и воспользуйтесь бесчисленными вариантами персонализации вашего клуба на поле и за его пределами. Играйте в одиночку в Squad Battles, вместе в кооп-игре FUT, онлайн в Division Rivals или против лучших игроков в FUT Champions, FIFA Ultimate Team\u2122 — это ваша связь с миром футбола на всём протяжении сезона.</span><br style=\"margin: 0px; padding: 0px; color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\"><br style=\"margin: 0px; padding: 0px; color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\"><span style=\"color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\">FIFA 23 даёт новое определение системе сыгранности в Ultimate Team, обеспечивая большую гибкость в подборе состава, и упрощает игру с друзьями благодаря впервые появившейся кроссплатформенной игре. А на футбольном поле вы сможете оценить совершенно новый способ игры в Моментах FUT — одиночных испытаниях, основанных на ситуациях из реальных матчей, — или играть и получать награды для вашего клуба в Кампаниях FUT.</span><br style=\"margin: 0px; padding: 0px; color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\"><br style=\"margin: 0px; padding: 0px; color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\"><span style=\"color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\">Женский футбол</span><br style=\"margin: 0px; padding: 0px; color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\"><br style=\"margin: 0px; padding: 0px; color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\"><span style=\"color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\">В FIFA 23 дебютирует женский клубный футбол, а это значит, что у вас будет возможность играть лучшими женскими командами мира.</span><br style=\"margin: 0px; padding: 0px; color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\"><span style=\"color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\">Благодаря технологии HyperMotion2, женский футбол в FIFA 23 получил новые реалистичные и специфические для женской игры анимации благодаря захвату движений в матче 11 на 11 в сочетании с передовым машинным обучением.</span><br style=\"margin: 0px; padding: 0px; color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\"><br style=\"margin: 0px; padding: 0px; color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\"><span style=\"color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\">При игре в FIFA 23 на ПК вы сможете пользоваться контроллером PlayStation и геймпадом Xbox и играть в своём стиле.</span><br style=\"margin: 0px; padding: 0px; color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\"><br style=\"margin: 0px; padding: 0px; color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\"><span style=\"color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\">В этой игре дополнительно продаётся виртуальная валюта, которую можно использовать для приобретения виртуальных внутриигровых товаров, включая случайный набор виртуальных игровых предметов.</span><br></p>"
                
            };
            fifa.Images.AddRange(new[]
            {
                new Image()
                {
                    Product = fifa,
                    ImgName = "c32f9d07-0dcd-4af9-bdcf-70f4b107d307.jpg"
                },
                new Image()
                {
                    Product = fifa,
                    ImgName = "bd681085-a50c-49c8-962b-380f73a94e9e.jpg"
                },
                new Image()
                {
                    Product = fifa,
                    ImgName = "029a1a97-ae3e-4a0c-bd58-35ca44b204ca.jpg"
                },
            });
            var granTurismo = new Product()
            {
                Name = "Gran Turismo 7",
                Translate = "Английский, Русский",
                Year = 2022,
                TransitTime = 40,
                ProductCategories = new List<ProductCategory>(new[] { race }),
                Description =
                    "<p><span style=\"color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\">Самый реалистичный симулятор вождения… 25 лет с вами.</span><br style=\"margin: 0px; padding: 0px; color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\"><br style=\"margin: 0px; padding: 0px; color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\"><span style=\"color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\">Садитесь за руль более 400 машин уже с первого дня и покоряйте 90 трасс в переменчивых погодных условиях. Вас ждут классические автомобили и ультрасовременные суперкары, воссозданные в мельчайших подробностях. Легендарный режим GT Simulation возвращается – откройте для себя новые машины и испытания, покупайте, улучшайте, гоняйте, продавайте автомобили в одиночной кампании.</span><br style=\"margin: 0px; padding: 0px; color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\"><br style=\"margin: 0px; padding: 0px; color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\"><span style=\"color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\">А если вы любите соревноваться с другими игроками, попробуйте свои силы в режиме GT Sport Mode.*</span><br style=\"margin: 0px; padding: 0px; color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\"><br style=\"margin: 0px; padding: 0px; color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\"><span style=\"color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\">Особенности игры на PlayStation\u00ae5:</span><br style=\"margin: 0px; padding: 0px; color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\"><span style=\"color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\">- Наблюдайте за другими водителями в зеркале заднего вида, смотрите, как солнечные лучи отражаются от вашего авто, в разрешении 4K и HDR, при 60 кадр/с.** Делайте невероятно реалистичные фото с трассировкой лучей на PS5\u2122.</span><br style=\"margin: 0px; padding: 0px; color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\"><span style=\"color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\">- Ощущайте вибрацию антиблокировочной системы и вращения руля, почувствуйте разницу между сопротивлением тормозов разных машин – благодаря адаптивным триггерам беспроводного контроллера DualSense\u2122.</span><br style=\"margin: 0px; padding: 0px; color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\"><span style=\"color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\">- Едва заметные неровности асфальта или стоки у обочины – ощущайте свое положение на дороге благодаря функции тактильной отдачи.</span><br style=\"margin: 0px; padding: 0px; color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\"><span style=\"color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\">- Чувствуйте положение других машин на дороге благодаря невероятно четкому 3D-звуку консоли PS5.</span><br></p>"
            };
            granTurismo.Images.AddRange(new[]
            {
                new Image()
                {
                    Product = granTurismo,
                    ImgName = "dd5eff76-80fc-41ce-9b0e-daf375c5552e.jpg"
                },
                new Image()
                {
                    Product = granTurismo,
                    ImgName = "f9f65241-0d93-48ec-96cd-0fb21d022a55.jpeg"
                },
                new Image()
                {
                    Product = granTurismo,
                    ImgName = "286d9356-212f-49e3-a873-cdc9bc4a854e.jpeg"
                },
            });
            
            var nfs = new Product()
            {
                Name = "Need for Speed Heat",
                Translate = "Английский, Русский",
                Year = 2019,
                TransitTime = 20,
                ProductCategories = new List<ProductCategory>(new[] { race }),
                Description =
                    "<p><span style=\"color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\">Устраивайте гонки днём и ставьте на кон всё ночью в Need for Speed\u2122 Heat — захватывающей игре, в которой вам предстоит столкнуться с лучшими силами полиции и покорить мир уличных гонок.</span><br></p>"
            };
            nfs.Images.Add(new Image()
            {
                ImgName = "d27bae9b-a61e-4298-ae3e-1d0a1649884b.jpg",
                Product = nfs
            });
            
            var spiderMan = new Product()
            {
                Name = "Spider-Man 2",
                Translate = "Английский, Русский",
                Year = 2023,
                TransitTime = 20,
                ProductCategories = new List<ProductCategory>(new[] { roleGames, actions }),
                Description =
                    "<p><span style=\"color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\">Людям-паукам Питеру Паркеру и Майлзу Моралесу предстоит окончательное испытание на прочность внутри и снаружи маски, сражаясь за спасение города, друг друга и тех, кого они любят, от чудовищного Венома и новой опасной угрозы симбиотов.</span><br style=\"margin: 0px; padding: 0px; color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\"><br style=\"margin: 0px; padding: 0px; color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\"><span style=\"color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\">Исследуйте обширный Нью-Йорк с помощью более быстрой паутины и совершенно новых Web Wings, быстро переключаясь между Питером и Майлзом, чтобы испытать разные истории, новые эпические способности и высокотехнологичное снаряжение.</span><br style=\"margin: 0px; padding: 0px; color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\"><br style=\"margin: 0px; padding: 0px; color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\"><span style=\"color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\">Используйте симбиотные способности Питера и взрывные биоэлектрические способности Майлза в битве против новых и знаковых суперзлодеев Marvel, включая оригинальный взгляд на Венома, наполненного симбиотами, безжалостного Крэйвена-Охотника, непостоянного Ящера и многих других из вселенной Marvel Rogues.</span><br style=\"margin: 0px; padding: 0px; color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\"><br style=\"margin: 0px; padding: 0px; color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\"><span style=\"color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\">Почувствуйте истинную силу Человека-Паука в своих руках!</span><br style=\"margin: 0px; padding: 0px; color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\"><span style=\"color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\">- Тактильная обратная связь: ощутите всю силу новых способностей Питера и Майлза у вас под рукой с помощью отзывчивой вибрации.</span><br style=\"margin: 0px; padding: 0px; color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\"><span style=\"color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\">- Адаптивные триггеры: осваивайте акробатические движения, выполняйте захватывающие комбо и откройте для себя захватывающий азарт перемещения по паутине.</span><br style=\"margin: 0px; padding: 0px; color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\"><br style=\"margin: 0px; padding: 0px; color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\"><span style=\"color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\">Живите жизнью супергероя!</span><br style=\"margin: 0px; padding: 0px; color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\"><span style=\"color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\">- Tempest 3D AudioTech: распознавайте звуки паутины, биоэлектрические силы, оживленное движение транспорта, отзывчивых жителей Нью-Йорка и опасные нападения врагов.*</span><br style=\"margin: 0px; padding: 0px; color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\"><span style=\"color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\">- Потрясающая графика: исследуйте новые красивые и яркие ландшафты и локации, включая Бруклин и Квинс, Кони-Айленд и другие.</span><br></p>"
            };
            spiderMan.Images.Add(new Image()
            {
                ImgName = "650e5564-8171-4145-9c55-a24c68065d06.jpg",
                Product = spiderMan
            });
            
            var gow = new Product()
            {
                Name = "God of War Ragnarok",
                Translate = "Английский, Русский",
                Year = 2022,
                TransitTime = 40,
                ProductCategories = new List<ProductCategory>(new[] { roleGames,actions }),
                Description =
                    "<p><span style=\"color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\">Отправляйтесь в эпическое и душевное путешествие, во время которого Кратос и Атрей должны научиться прощать и не падать духом.</span><br style=\"margin: 0px; padding: 0px; color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\"><span style=\"color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\">Студия Santa Monica подготовила продолжение знаменитой игры God of War (2018 год). Фимбулвинтер уже близко. Кратос и Атрей должны отправиться во все Девять миров и найти ответы, пока войска Асгарда готовятся к предречённой битве, которая положит конец мирозданию. Им предстоит побывать в мифических краях и сойтись в бою с бесстрашными врагами в лице скандинавских богов и чудовищ. Рагнарёк еще никогда не был так близок. Судьба поставит Кратоса и Атрея перед выбором между собственной безопасностью и безопасностью всех миров.</span><br style=\"margin: 0px; padding: 0px; color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\"><span style=\"color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\">Ненаписанное будущее</span><br style=\"margin: 0px; padding: 0px; color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\"><span style=\"color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\">Атрей жаждет знаний, которые помогут ему разгадать пророчество «Локи» и понять, какова его участь в Рагнарёке. Кратос должен решить: поддаться сковывающему его страху не повторить ошибки прошлого или вырваться из своих кошмарных воспоминаний и стать отцом, в котором нуждается Атрей.</span><br style=\"margin: 0px; padding: 0px; color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\"><span style=\"color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\">Орудие войны</span><br style=\"margin: 0px; padding: 0px; color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\"><span style=\"color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\">Топор Левиафан, клинки Хаоса и щит стража возвращаются вместе с новыми способностями Кратоса и Атрея. В боях с богами и чудовищами в девяти мирах спартанские навыки Кратоса, защищающего свою семью, подвергнутся прежде невиданному испытанию.</span><br style=\"margin: 0px; padding: 0px; color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\"><span style=\"color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\">Исследуйте необъятные миры</span><br style=\"margin: 0px; padding: 0px; color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\"><span style=\"color: rgb(85, 85, 85); font-family: roboto, sans-serif; font-size: 15px;\">Путешествуйте по живописным, но опасным просторам, сражайтесь с разнообразными существами, чудищами и скандинавскими богами. Помогите Кратосу и Атрею найти ответы.</span><br></p>"
                
            };
            gow.Images.AddRange(new[]
            {
                new Image()
                {
                    Product = gow,
                    ImgName = "bbb75c2d-8b83-42ee-8c23-bb6954e1abde.jpg"
                },
                new Image()
                {
                    Product = gow,
                    ImgName = "57583b95-7106-42e5-a0b1-96104d782f78.jpg"
                },
                new Image()
                {
                    Product = gow,
                    ImgName = "efb5be53-49f4-443c-bfcc-43f519d836aa.jpg"
                },
            });
            

            await db.Products.AddRangeAsync(new[]
            {
                theWitcher,
                fifa,
                granTurismo,
                nfs,
                spiderMan,
                gow
            });

            await db.Images.AddRangeAsync(theWitcher.Images);
            await db.Images.AddRangeAsync(fifa.Images);
            await db.Images.AddRangeAsync(granTurismo.Images);
            await db.Images.AddRangeAsync(nfs.Images);
            await db.Images.AddRangeAsync(spiderMan.Images);
            await db.Images.AddRangeAsync(gow.Images);

            var consoles = db.Consoles.ToList();
            
            //игровые аккаунты
            var wither1 = new Account()
            {
                AccountNumber = 1,
                Login = "exampleLogin",
                Password = "Pass123$",
                Name = "Ведьмак 3: Дикая Охота",
                Price = 199,
                Product = theWitcher,
                Consoles = new List<Console>(consoles)
            };
            var wither2 = new Account()
            {
                AccountNumber = 2,
                Login = "exampleLogin",
                Password = "Pass123$",
                Name = "Ведьмак 3: Дикая Охота",
                Price = 250,
                Product = theWitcher,
                Consoles = new List<Console>(new []{consoles[1]})
            };
            
            var fifa1 = new Account()
            {
                AccountNumber = 1,
                Login = "LoginForFifa",
                Password = "Pass123$",
                Name = "Fifa 23",
                Price = 99,
                Product = fifa,
                Consoles = new List<Console>(consoles)
            };
            var fifa2 = new Account()
            {
                AccountNumber = 2,
                Login = "LoginForFifa",
                Password = "Pass123$",
                Name = "Fifa 23",
                Price = 99,
                Product = fifa,
                Consoles = new List<Console>(consoles)
            };
            var fifa3 = new Account()
            {
                AccountNumber = 3,
                Login = "LoginForFifa",
                Password = "Pass123$",
                Name = "Fifa 23",
                Price = 250,
                Product = fifa,
                Consoles = new List<Console>(consoles)
            };
            var granTurismo1 = new Account()
            {
                AccountNumber = 1,
                Login = "LoginForGranturismo",
                Password = "Pass123$",
                Name = "Gran Turismo 7",
                Price = 99,
                Product = granTurismo,
                Consoles = new List<Console>(consoles)
            };
            var nfs1 = new Account()
            {
                AccountNumber = 1,
                Login = "LoginForNFS",
                Password = "Pass123$",
                Name = "Need For Speed Heat",
                Price = 499,
                Product = nfs,
                Consoles = new List<Console>(consoles)
            };
            
            var spiderMan1 = new Account()
            {
                AccountNumber = 1,
                Login = "LoginForSpiderMan",
                Password = "Pass123$",
                Name = "Spider Man 2",
                Price = 300,
                Product = spiderMan,
                Consoles = new List<Console>(consoles)
            };
            
            var gow1 = new Account()
            {
                AccountNumber = 1,
                Login = "LoginForGOW",
                Password = "Pass123$",
                Name = "God Of War Ragnarok",
                Price = 199,
                Product = gow,
                Consoles = new List<Console>(consoles)
            };

            await db.Accounts.AddRangeAsync(new[]
            {
                wither1,
                wither2,
                fifa1,
                fifa2,
                fifa3,
                granTurismo1,
                nfs1,
                spiderMan1,
                gow1
            });
            
            await db.SaveChangesAsync();
            
            Log.Debug("Add data to database");
        }
    }
}