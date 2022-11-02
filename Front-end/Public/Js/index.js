import Wallet from "./wallet.js"; 
import Block from "./block.js";
import WalletInfo from "./walletInfo.js";
// FormatDate like 2022-11-02 21:03:05
function FormatDate() {

    var date = new Date();
    var aaaa = date.getUTCFullYear();
    var gg = date.getUTCDate();
    var mm = (date.getUTCMonth() + 1);

    if (gg < 10)
        gg = "0" + gg;

    if (mm < 10)
        mm = "0" + mm;

    var cur_day = aaaa + "-" + mm + "-" + gg;

    var hours = date.getUTCHours()
    var minutes = date.getUTCMinutes()
    var seconds = date.getUTCSeconds();

    if (hours < 10)
        hours = "0" + hours;

    if (minutes < 10)
        minutes = "0" + minutes;

    if (seconds < 10)
        seconds = "0" + seconds;

    return cur_day + " " + hours + ":" + minutes + ":" + seconds;

}

const loginBtn = document.getElementById("btnLogin");

loginBtn.addEventListener("click", (event) => {
    const inputFieldLogin = document.getElementById("loginPrivateKey").value;

    if(inputFieldLogin.toString().length == 0) {
        alert("Please enter a private key");
    }else{
        localStorage.setItem("loginKey", inputFieldLogin);  
        console.log(localStorage.getItem("loginKey")); 

    }  
});

const wallet ={
    mywallet: document.querySelector("#mywallet"),
    searchWallet: document.querySelector("#searchWalletButton"),
}
// wallet.mywallet.addEventListener("click", console.log(mywallet));

const block ={
    searchBlock: document.querySelector("#searchBlockButton"),
}

const walletInfo = {
    
}


