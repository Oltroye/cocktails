var express = require('express');
const ipApiManager = require('../class/ipApiManager');
const security = require('../class/security');
var router = express.Router();

router.get('/:id', async function(req, res, next) {
    const userId = req.params.id;
    if (userId && userId.trim() !== '') {
        ipManager = await ipApiManager.getInstance();
        ip = ipManager.get();
        listuser = security.getInstance();
        id = listuser.get(userId)
        // add get coktails
        const getCoktails = new Request("http://"+ip+"/api/cocktail");
        fetch(getCoktails)
        .then(response => {
            if (!response.ok) {
                // error during the request
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            return response.json();
        })
        .then( async data => {
            var cocktails = data;
            var cocktailsToSend = [];

            var ingridentsUser = [];
            const getUserIngridients = new Request("http://"+ip+"/api/userIngredients/"+id);
            await fetch(getUserIngridients)
            .then(response => {
                if (!response.ok) {
                    // error during the request
                    throw new Error(`HTTP error! status: ${response.status}`);
                }
                return response.json();
            })
            .then(async data => {
                ingridentsUser = data;
            });

            
            userChoice = []
            const getUserChoice = new Request("http://"+ip+"/api/userChoice/"+id);
            await fetch(getUserChoice)
            .then(response => {
                if (!response.ok) {
                    // error during the request
                    throw new Error(`HTTP error! status: ${response.status}`);
                }
                return response.json();
            })
            .then( data => {
                userChoice = data
            });
            console.log(userChoice)

            for(index = 0; index < cocktails.length; index++) {
                console.log(index)
                listId = security.getInstance();
                key = listId.add(cocktails[index].idCocktail);

                var ingridentsCoctail = [];
                const getCocktailIngridients = new Request("http://"+ip+"/api/cocktailIngredient/"+cocktails[index].idCocktail);
                await fetch(getCocktailIngridients)
                .then(response => {
                    if (!response.ok) {
                        // error during the request
                        throw new Error(`HTTP error! status: ${response.status}`);
                    }
                    return response.json();
                })
                .then( data => {
                    ingridentsCoctail = data;
                });

                count = 0;
                for ( ingridentCoctail in ingridentsCoctail) {
                    for (ingridentUser in ingridentsUser) {
                        if (ingridentsCoctail[ingridentCoctail].idIngredient == ingridentsUser[ingridentUser].idIngredients && ingridentsUser[ingridentUser].isOwned) {
                            count++;
                        }
                    }
                }

                chose = false
                for (choice in userChoice) {
                    if (userChoice[choice].idCocktail == cocktails[index].idCocktail) {
                        chose = true;
                        break;
                    }
                }

                var coctail = {
                    "idCocktail": key,
                    "name": cocktails[index].name,
                    "picture": cocktails[index].picture,
                    "glassType": cocktails[index].glassType,
                    "description": cocktails[index].description,
                    "alcoolFree": cocktails[index].alcoolFree,
                    "difficulty": cocktails[index].difficulty,
                    "popularity": cocktails[index].popularity,
                    "recipe": cocktails[index].recipe,
                    "preparationTime": cocktails[index].preparationTime,
                    "makable": (count == ingridentsCoctail.length),
                    "chose": chose 
                }
                cocktailsToSend.push(coctail);
            }
            
            res.render('cocktails', { title: "Cocktail's", conected: true,id: userId, cocktailList: cocktailsToSend});
        });
    } else {
        res.redirect('/');
    }
});

router.post('/:idUser/add/:idCoctail', async function(req,res, next) {
    const userId = req.params.idUser;
    const idCock = req.params.idCoctail;
    listId = security.getInstance();

    ipManager = await ipApiManager.getInstance();
    ip = ipManager.get();

    const userChoice = {
        "idUser": listId.get(userId),
        "idCocktail": listId.get(idCock)
    };
    const request = new Request("http://"+ip+"/api/userChoice", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(userChoice),
    });
    const response = await fetch(request);
    if (!response.ok) {
        // ingredient not found (la encore j'ai oublié la redirection)
        const errorData = await response.json();
        return res.status(response.status).send(errorData.message || "Registration failed");
    }
    
    res.redirect('/cocktails/'+userId);
});

router.post('/:idUser/remove/:idCoctail', async function(req,res, next) {
    const userId = req.params.idUser;
    const idCock = req.params.idCoctail;
    listId = security.getInstance();

    ipManager = await ipApiManager.getInstance();
    ip = ipManager.get();

    const userChoice = {
        "idUser": listId.get(userId),
        "idCocktail": listId.get(idCock)
    };
    const request = new Request("http://"+ip+"/api/userChoice/"+listId.get(userId)+"/"+listId.get(idCock), {
        method: "DELETE"
    });
    const response = await fetch(request);
    if (!response.ok) {
        // ingredient not found (la encore j'ai oublié la redirection)
        const errorData = await response.json();
        return res.status(response.status).send(errorData.message || "Registration failed");
    }
    
    res.redirect('/cocktails/'+userId);
});

module.exports = router;