import Wallet from "./wallet.js"; 
import Block from "./block.js";
import WalletInfo from "./walletInfo.js";

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


