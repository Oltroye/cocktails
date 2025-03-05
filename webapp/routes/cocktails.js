var express = require('express');
var router = express.Router();

router.get('/:id', function(req, res, next) {
    const userId = req.params.id;
    if (userId && userId.trim() !== '') {
        // add check user exist
        res.render('cocktails', {id: userId});
    } else {
        res.redirect('/');
    }
});

module.exports = router;