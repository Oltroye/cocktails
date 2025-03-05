var express = require('express');
var router = express.Router();

/* GET home page. */
router.get('/', function(req, res, next) {
  res.render('signup', { title: 'Signup' });
});

router.post('/', function(req,res, next) {
  const { username, name, lastname, password } = req.body;
  console.log(username, name, lastname, password)
});

module.exports = router;
