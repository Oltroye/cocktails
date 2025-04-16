var express = require('express');
var router = express.Router();
var security = require('../class/security');

router.get('/:id', function(req, res, next) {
    const userId = req.params.id;
    if (userId && userId.trim() !== '') {
        // add check user exist
        listuser = security.getInstance();
        id = listuser.get(userId)
        res.render('home', { title: "Cocktail's", id: id });
    } else {
        res.redirect('/');
    }
});

module.exports = router;