var express = require('express');
var router = express.Router();
const ipApiManager = require('../class/ipApiManager');
const security = require('../class/security');

router.get('/:id', async function(req, res, next) {
    const userId = req.params.id;

    if (userId && userId.trim() !== '') {
        ipManager = await ipApiManager.getInstance();
        ip = ipManager.get();
        listuser = security.getInstance();
        id = listuser.get(userId)
        // get useringredients
        const getUserIngridients = new Request("http://"+ip+"/api/userIngredients/"+id);
        fetch(getUserIngridients)
        .then(response => {
            if (!response.ok) {
                // error during the request
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            return response.json();
        })
        .then(async data => {
            var userIngridients = data;
            var ingrediants = [];
            for (const userIngredient of userIngridients) {
                console.log(userIngredient)
                console.log(userIngredient.idIngredients)
                const getIngridients = new Request("http://"+ip+"/api/ingredients/"+userIngredient.idIngredients);
                await fetch(getIngridients)
                .then(response => {
                    if (!response.ok) {
                        // error during the request
                        throw new Error(`HTTP error! status: ${response.status}`);
                    }
                    return response.json();
                })
                .then(data => {
                    console.log(data)
                    ingrediants.push(data)
                });
            };
            var Types = [];
            for (const ingrediant of ingrediants) {
                const getType = new Request("http://"+ip+"/api/type/"+ingrediant.idType);
                await fetch(getType)
                .then(response => {
                    if (!response.ok) {
                        // error during the request
                        throw new Error(`HTTP error! status: ${response.status}`);
                    }
                    return response.json();
                })
                .then(data => {
                    Types.push(data)
                });
            };
            console.log(userIngridients)
            console.log(ingrediants)
            console.log(Types)
            var listIngridients = [];
            for(index = 0; index < userIngridients.length; index++) {
                var ingrediant = {
                    "idIngredients": userIngridients[index].idIngredients,
                    "isOwned": userIngridients[index].isOwned,
                    "name": ingrediants[index].name,
                    "nameType": Types[index].name
                }
                listIngridients.push(ingrediant);
            }

            res.render('stock', { title: "Cocktail's", id: userId, ingridients: listIngridients});
        });
    } else {
        res.redirect('/');
    }
});

module.exports = router;