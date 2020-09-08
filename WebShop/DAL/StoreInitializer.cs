using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using WebShop.Models;

namespace WebShop.DAL
{
    public class StoreInitializer : DropCreateDatabaseIfModelChanges<StoreContext>
    {
        protected override void Seed(StoreContext context)
        {
            SeedStoreData(context);

            base.Seed(context);
        }

        private void SeedStoreData(StoreContext context)
        {
            var kategoria = new List<Kategoria>
            {
                new Kategoria() {KategoriaId = 1, Name = "Kolorowe"},
                new Kategoria() {KategoriaId = 2, Name = "Gładkie"},
                new Kategoria() {KategoriaId = 3, Name = "Zimowe"},
                new Kategoria() {KategoriaId = 4, Name = "Letnie"},

            };

            kategoria.ForEach(k => context.Kategorie.Add(k));
            context.SaveChanges();

            var produkty = new List<Produkt>
            {
                new Produkt() { ProduktId = 1, Nazwa = "Pug Life", Cena=20, KategoriaId=1,Opis="Hau, hau! Oto skarpetki w pieski dla miłośników mopsów! Idealne na prezent, urocze i słodkie – dokładnie jak mopsy. Skarpetki w mopsy.", DataDodania = new DateTime(2020,09,9),NazwaObrazka="Pug_Life.jpg"},
                new Produkt() { ProduktId = 2, Nazwa = "Piwo", Cena=19, KategoriaId=1,Opis="Skarpetki Chmielowo Mi to idealny model dla koneserów dobrego piwa. Szukasz zabawnych skarpetek na spotkania z przyjaciółmi? Właśnie je znalazłeś :) Elegancka, nieco dekadencka czerń doskonale współgr.", DataDodania = new DateTime(2020,09,9),NazwaObrazka="piwo.png"},
                new Produkt() { ProduktId = 3, Nazwa = "Monkey Business", Cena=25, KategoriaId=1,Opis="Skarpetki przeznaczone są dla kobiet, dziewczyn, mężczyzn i chłopców. Płaski szew na palcach daje najlepszy komfort noszenia.", DataDodania = new DateTime(2020,09,9),NazwaObrazka="monkey.png"},
                new Produkt() { ProduktId = 4, Nazwa = "Czarne", Cena=15, KategoriaId=2,Opis="Gładkie czarne.", DataDodania = new DateTime(2020,09,9),NazwaObrazka="czarne.jpg"},
                new Produkt() { ProduktId = 5, Nazwa = "Białe", Cena=15, KategoriaId=2,Opis="Gładkie białe.", DataDodania = new DateTime(2020,09,9),NazwaObrazka="biale.jpg"},
                new Produkt() { ProduktId = 6, Nazwa = "Skiing", Cena=18, KategoriaId=3,Opis="Zimowe szaleństwo przez cały rok! Nie wierzysz? Sprawdź nasze kolorowe skarpetki dla zapalonych narciarzy!", DataDodania = new DateTime(2020,09,9),NazwaObrazka="Skiing.jpg"},
                new Produkt() { ProduktId = 6, Nazwa = "Skiing", Cena=18, KategoriaId=3,Opis="Zimowe szaleństwo przez cały rok! Nie wierzysz? Sprawdź nasze kolorowe skarpetki dla zapalonych narciarzy!", DataDodania = new DateTime(2020,09,9),NazwaObrazka="Skiing.jpg"},
                new Produkt() { ProduktId = 7, Nazwa = "Vege Salad", Cena=18, KategoriaId=4,Opis="Jeśli lubisz kolorowe sałatki, z pewnością lubisz też kolorowe skarpetki! Dziś mamy dla Ciebie zdrowe połączenie.", DataDodania = new DateTime(2020,09,9),NazwaObrazka="Vege_Salad.jpg"},
                new Produkt() { ProduktId = 8, Nazwa = "Fluffy Alpaca", Cena=18, KategoriaId=4,Opis="Skarpetki przeznaczone są dla kobiet, dziewczyn, mężczyzn i chłopców. Płaski szew na palcach.", DataDodania = new DateTime(2020,09,9),NazwaObrazka="Fluffy_Alpaca.png"},
                new Produkt() { ProduktId = 9, Nazwa = "Warm Rudolph", Cena=18, KategoriaId=3,Opis="Skarpetki przeznaczone są dla kobiet, dziewczyn, mężczyzn i chłopców. Płaski szew na palcach.", DataDodania = new DateTime(2020,09,9),NazwaObrazka="Warm_Rudolph.jpg"},
                new Produkt() { ProduktId = 10, Nazwa = "Speedway", Cena=18, KategoriaId=4,Opis="Poznaj nasze nowe skarpetki żużlowe! Patrząc na nie, usłyszysz ryk motoru żużlowego i doping kibiców.", DataDodania = new DateTime(2020,09,9),NazwaObrazka="Speedway.jpeg"},
                new Produkt() { ProduktId = 11, Nazwa = "Wspinaczka", Cena=18, KategoriaId=4,Opis="Skarpetki przeznaczone są dla kobiet, dziewczyn, mężczyzn i chłopców. Płaski szew na palcach.", DataDodania = new DateTime(2020,09,9),NazwaObrazka="Wspinaczka.jpeg"},
                new Produkt() { ProduktId = 12, Nazwa = "Burger", Cena=18, KategoriaId=1,Opis="Masz ochotę na coś smakowitego, a pomysłu na obiad ciągle brak? Mamy na to radę!", DataDodania = new DateTime(2020,09,9),NazwaObrazka="Burger.jpg"},
                new Produkt() { ProduktId = 13, Nazwa = "Pingwiny", Cena=18, KategoriaId=3,Opis="Skarpetki przeznaczone są dla kobiet, dziewczyn, mężczyzn i chłopców. Płaski szew na palcach.", DataDodania = new DateTime(2020,09,9),NazwaObrazka="Pingwiny.jpg"},
                new Produkt() { ProduktId = 14, Nazwa = "Oh Deer!", Cena=18, KategoriaId=3,Opis="Idealne, wygodne, miękkie, ciepłe, puszyste, bajecznie kolorowe skarpetki zimowe – myślisz, że nie istnieją?", DataDodania = new DateTime(2020,09,9),NazwaObrazka="Oh_Deer.jpg"}
            };

            produkty.ForEach(p => context.Produkty.Add(p));
            context.SaveChanges();

        }
    }
}