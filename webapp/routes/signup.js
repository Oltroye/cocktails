var express = require('express');
var router = express.Router();
var security = require('../class/security');

/* GET home page. */
router.get('/', function(req, res, next) {
  res.render('signup', { title: 'Signup' });
});

router.post('/', function(req,res, next) {
  const { username, name, lastname, password } = req.body;
  newkey = security.getInstance();
  key = newkey.add(username)
  res.redirect('/home/'+key)
});

module.exports = router;