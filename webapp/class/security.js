class Security {
    static instance;
    annuary = new Map();
    limitDate = new Map();

    static getInstance() {
        if (!this.instance) {
            this.instance = new Security();
        }
        return this.instance;
    }

    get(key) {
        clean();
        if(this.annuary.has(key)) {
            return this.annuary.get(key);
        }
        return null;
    }

    add(value) {
        let key;
        do {
            key = this.generateKey();
        } while (this.annuary.has(key));
        
        const expirationTime = Date.now() + 24 * 60 * 60 * 1000; // 24h en millisecondes
        this.annuary.set(key, value);
        this.limitDate.set(key, expirationTime);
        
        return key;
    }

    isValid(key) {
        const expiration = this.limitDate.get(key);
        return expiration && expiration > Date.now();
    }

    deleteKey(key) {
        this.annuary.delete(key);
        this.limitDate.delete(key);
    }

    generateKey(length = 5) {
        const characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
        let key = '';
      
        for (let i = 0; i < length; i++) {
          key += characters.charAt(Math.floor(Math.random() * characters.length));
        }
      
        return key;
    }

    clean() {
        for (var key of sayings) {
            if (!this.isValid(key)) {
                this.deleteKey(key);
            }
        }
    }
}

module.exports = Security;