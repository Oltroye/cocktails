using cocktails.Models;
using cocktails.Lists;


namespace cocktails.Lists
{
    public class ListCocktails
    {
        public Cocktail[] List => new[]
        {
            new Cocktail
            {
                Name = "Mojito",
                Picture = "",
                GlassType = "verre highball",
                Description = "Cocktail rafraîchissant idéal pour l'été. Utilise de la menthe fraîche pour un goût optimal.",
                AlcoolFree = false,
                Difficulty = "Facile",
                Popularity = 3,
                Recipe = "\"Dans un verre, écraser les feuilles de menthe avec le sucre et le jus de citron vert.\r\nAjouter le rhum et mélanger.\r\nRemplir le verre de glace, puis compléter avec de l'eau gazeuse.\r\nGarnir avec une branche de menthe et une rondelle de citron vert.\"",
                PreparationTime = 10,
            },
            new Cocktail
            {
                Name = "Margarita",
                Picture = "",
                GlassType = "verre à margarita",
                Description = "Un classique mexicain. Pour un twist, ajoute un peu de purée de fruits.",
                AlcoolFree = false,
                Difficulty = "Facile",
                Popularity = 3,
                Recipe = "\"Humidifier le bord du verre avec du citron et tremper dans le sel.\r\nMélanger tous les ingrédients dans un shaker avec de la glace.\r\nSecouer et filtrer dans le verre préparé.\"",
                PreparationTime = 5,
            },
            new Cocktail
            {
                Name = "Piña Colada",
                Picture = "",
                GlassType = "verre à cocktail",
                Description = "Évasion tropicale garantie. Parfait pour les journées ensoleillées !",
                AlcoolFree = false,
                Difficulty = "Facile",
                Popularity = 3,
                Recipe = "\"Mixer tous les ingrédients avec de la glace jusqu'à obtenir une consistance lisse.\r\nServir dans un verre et garnir d'une tranche d'ananas.\"",
                PreparationTime = 5,
            },
            new Cocktail
            {
                Name = "Bloody Mary",
                Picture = "",
                GlassType = "verre highball",
                Description = "Cocktail parfait pour le brunch. Ajuste les épices selon ton goût !",
                AlcoolFree = false,
                Difficulty = "Moyenne",
                Popularity = 3,
                Recipe = "\"Dans un shaker, mélanger tous les ingrédients avec de la glace.\r\nFiltrer dans un verre rempli de glace.\r\nGarnir avec une branche de céleri ou un cornichon.\"",
                PreparationTime = 5,
            },
        };
    }
}
