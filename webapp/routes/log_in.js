var express = require('express');
var router = express.Router();
var security = require('../class/security');

router.get('/', function(req, res, next) {
  res.render('log_in', { title: "Cocktail's" });
});

router.post('/', function(req,res, next) {
  const { username, password } = req.body;
  newkey = security.getInstance();
  key = newkey.add(username)
  res.redirect('/home/'+key)
});

module.exports = router;