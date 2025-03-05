var express = require('express');
var router = express.Router();

router.get('/:id', function(req, res, next) {
    const userId = req.params.id;
    console.log(userId)
    if (userId && userId.trim() !== '') {
        res.render('home', { title: "Cocktail's", id: userId });
    } else {
        res.redirect('/');
    }
});

module.exports = router;