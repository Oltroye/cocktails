var express = require('express');
var router = express.Router();
var security = require('../class/security');
const ipApiManager = require('../class/ipApiManager');
const bcrypt = require('bcrypt');

/* GET home page. */
router.get('/', function(req, res, next) {
  res.render('signup', { title: 'Signup' });
});

router.post('/', async function(req,res, next) {
  const { username, email, name, firstname, password, passwordbis } = req.body;
  // create new user
  if (password !== passwordbis || !username || !email || !name) {
    // error pasword dosent match or empty element in form
    res.redirect('/sign_up')
  }
  const salt = await bcrypt.genSalt();
  const hashedPassword = await bcrypt.hash(password, salt);
    
  ipManager = await ipApiManager.getInstance();
  ip = ipManager.get();

  const user = {
    "userName": username,
    "email": email,
    "password": hashedPassword,
    "name": name,
    "firstName": firstname
  };

  const request = new Request("http://"+ip+"/api/user", {
    method: "POST",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify(user),
  });
  const response = await fetch(request);
  if (!response.ok) {
    //email already used (fait gafe j'ai pas mis de redirection ca crash)
    const errorData = await response.json();
    return res.status(response.status).send(errorData.message || "Registration failed");
  }

  // Handle successful response
  const data = await response.json();
  newkey = security.getInstance();
  key = newkey.add(data.idUser)
  res.redirect('/cocktails/'+key)

});

module.exports = router;