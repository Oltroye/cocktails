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
        // add check user exist
        res.render('stock', { title: "Cocktail's", id: id });
    } else {
        res.redirect('/');
    }
});

module.exports = router;