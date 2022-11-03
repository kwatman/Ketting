export default class Wallet{
    constructor(publicKey,balance){
        this._publicKey = publicKey;
        this._balance = balance;
    }

    get publicKey(){return this._publicKey;}
    get balance(){return this._balance;}

    toHtmlString(){
        return `
            <li class="list-group-item" id="">${this.publicKey}" "${this.balance}</li>
        `
    }
}