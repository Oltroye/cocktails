var express = require('express');
var router = express.Router();

router.get('/', function(req, res, next) {
  res.render('log_in', { title: "Cocktail's" });
});

router.post('/', function(req,res, next) {
    const { username, password } = req.body;
    console.log(username, password)
    res.redirect('/home/'+username);
});

module.exports = router;