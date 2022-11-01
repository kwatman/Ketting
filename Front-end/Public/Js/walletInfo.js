export default class WalletInfo{
    constructor(transactionKeyHistory, transactionAmountHistory){
        this._transactionKeyHistory = transactionKeyHistory;
        this._transactionAmountHistory = transactionAmountHistory;
    }

    get transactionKeyHistory() {return this._transactionKeyHistory}
    get transactionAmountHistory() {return this._transactionAmountHistory}

    toHtmlString(){
        `
        <li class="list-group-item" id="">${this.transactionKeyHistory}" "${this.transactionAmountHistory}</li>
        `
    }
}