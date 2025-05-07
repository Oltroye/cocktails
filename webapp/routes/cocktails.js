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
        res.render('cocktails', { title: "Cocktail's", conected: true,id: userId});
    } else {
        res.redirect('/');
    }
});

module.exports = router;