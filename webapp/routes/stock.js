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
            var listIngridients = [];
            for(index = 0; index < userIngridients.length; index++) {
                listId = security.getInstance();
                key = listId.add(userIngridients[index].idIngredients);
                var ingrediant = {
                    "idIngredients": key,
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


router.post('/:idUser/unowned/:idIngredient', async function(req,res, next) {
    const userId = req.params.idUser;
    const idIng = req.params.idIngredient;
    listId = security.getInstance();

    ipManager = await ipApiManager.getInstance();
    ip = ipManager.get();

    const userIngredient = {
        "isOwned": false
    };
    const request = new Request("http://"+ip+"/api/userIngredients/"+listId.get(userId)+"/"+listId.get(idIng), {
        method: "PUT",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(userIngredient),
    });
    const response = await fetch(request);
    if (!response.ok) {
        // ingredient not found (la encore j'ai oublié la redirection)
        const errorData = await response.json();
        return res.status(response.status).send(errorData.message || "Registration failed");
    }
    
    res.redirect('/stock/'+userId);
});

router.post('/:idUser/owned/:idIngredient', async function(req,res, next) {
    const userId = req.params.idUser;
    const idIng = req.params.idIngredient;
    listId = security.getInstance();

    ipManager = await ipApiManager.getInstance();
    ip = ipManager.get();

    const userIngredient = {
        "isOwned": true
    };

    const request = new Request("http://"+ip+"/api/userIngredients/"+listId.get(userId)+"/"+listId.get(idIng), {
        method: "PUT",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(userIngredient),
    });
    const response = await fetch(request);
    if (!response.ok) {
        // ingredient not found (la encore j'ai oublié la redirection)
        const errorData = await response.json();
        return res.status(response.status).send(errorData.message || "Registration failed");
    }
    
    res.redirect('/stock/'+userId);
});

module.exports = router;