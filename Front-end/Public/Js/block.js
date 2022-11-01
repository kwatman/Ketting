export default class Block{
    constructor(key, date){
        this._key = key;
        this._date = date;
    }

    get key() {return this._key;}
    get date() {return this._date}

    toHtmlString(){
        return `
        <li class="list-group-item" id="">${this.key}" "${this.date}</li>
    `
    }
}