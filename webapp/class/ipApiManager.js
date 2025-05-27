const fs = require('fs');

class IpApiManager {
    static instance;

    constructor() {
        this.ip = null;
    }

    static async getInstance() {
        if (!this.instance) {
            this.instance = new IpApiManager();
            const data = await fs.promises.readFile('params/ip_api.txt');
            this.instance.ip = data.toString();
        }
        return this.instance;
    }

    get() {
        return this.ip;
    }
}

module.exports = IpApiManager;