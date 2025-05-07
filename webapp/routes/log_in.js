var express = require('express');
var router = express.Router();
var security = require('../class/security');
const ipApiManager = require('../class/ipApiManager');
const bcrypt = require('bcrypt');

router.get('/', function(req, res, next) {
  res.render('log_in', { title: "Cocktail's" });
});

router.post('/', async function(req,res, next) {
  const { email, password } = req.body;
  if (email == "" || password == "") {
    // error empty form champ
    res.redirect('/log_in')
  }
  ipManager = await ipApiManager.getInstance();
  ip = ipManager.get();
  const userinformation = new Request("http://"+ip+"/api/user/Login/"+email);
  // check user identification
  fetch(userinformation)
  .then(response => {
    if (!response.ok) {
      // error during the request
      throw new Error(`HTTP error! status: ${response.status}`);
    }
    return response.json();
  })
  .then(async data => {
    console.log(data);
    const passwordMatched = await bcrypt.compare(password, data.password);
    if (passwordMatched) {
      newkey = security.getInstance();
      key = newkey.add(data.idUser)
      res.redirect('/cocktails/'+key)
    } else {
      // add error message wrong password
      res.redirect('/log_in');
    }
  })
  .catch(error => {
    // add error message user not found
    console.error('Fetch error:', error);
    res.redirect('/log_in')
  });
});

module.exports = router;