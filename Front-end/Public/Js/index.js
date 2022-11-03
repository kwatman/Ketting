import Wallet from "./wallet.js"; 
import Block from "./block.js";
import WalletInfo from "./walletInfo.js";

const loginBtn = document.getElementById("btnLogin");
let count = 0;
const completeTranscaction = document.getElementById("completeTranscaction")
const searchBlock = document.querySelector("#searchBlockButton")
const mywallet =document.querySelector("#mywallet")
const searchWallet = document.querySelector("#searchWalletButton")

if(loginBtn != null){
loginBtn.addEventListener("click", async(e) => {
    e.preventDefault();
    const loginPrivate = document.getElementById("loginPrivateKey").value;
    const loginPublic = document.getElementById("loginPublicKey").value;
    if(loginPrivate.toString().length == 0 || loginPublic.toString().length == 0) {
        alert("Please enter a private key and public key");
    }else{
        localStorage.setItem("privateKey", loginPrivate); 
        localStorage.setItem("publicKey", loginPublic);  
        console.log("Private Key: " + localStorage.getItem("privateKey") + "// Public Key: " + localStorage.getItem("publicKey")); 
    }  
})};


if(completeTranscaction != null){
    completeTranscaction.addEventListener("click", async(e)=>{
        e.preventDefault();
        count++
        const amount= document.getElementById("amount").value;
        const receiver = document.getElementById("receiver").value;
        const datum = Date.now();
        const loginPrivate = localStorage.getItem("privateKey");
        const loginPublic = localStorage.getItem("publicKey");
        const string="SgLYNAIOlfMOSYOn1nojnD4XesO1SX26MUi/XOJm8tG9zZfZ5HxhjoM7s2PgIkQW0BmU/JIhrvQWf5Ul1g5hQnvWC/LavHso3XgjrZ7937xyzD+mSPq0f93YaIurrYq+FJPCRVKLy2XQ7673+4PWj41Td2jxKBNUw9Z7GcW3jlnoqYP6BXaiwwlU8f2BRBPxfK8LDrJhy5W9BDq/SqIZcnjNkqFch5xYrNUNPRzkbql9LP7pSDjO9L1LiKrRBAOy8CDUuCNgwLMoEKyGccdw22GDBb3nP+SU/f4fsI7qJTndgHCQP0qTg0Y7W8dPBJHGr+ZdxRTLpzSKgLW75JSR0w=="
        let signNotEncrypt = localStorage.getItem("publicKey") + "@" + receiver + "@" + amount + "@" + datum;

        if(amount.toString().length >0 && receiver.toString().length > 0){
            
        // var sign = new JSEncrypt();
        // sign.setPrivateKey(loginPrivate.val());
        // var signature = sign.sign(signNotEncrypt.val(), CryptoJS.SHA256, "sha256");

        let transaction = {
            "transactionNumber" : count,
            "amount" : amount,
            "timeStamp" : "2022-11-03T11:18:00",
            "senderKey" : loginPublic,
            "receiverKey" : receiver,
            "signature" : string,
            "type" : 0
        }
        await fetch("http://localhost:7262/transaction",{
            method: "POST",
            header: {"Content-Type":"application/json"},
            body: JSON.stringify(transaction)   
        }).then(async(res) => { 
                alert("Transaction of " + amount + " has been completed");
        });
        }else{alert("Fill the inputfields to make transaction");}
    
        console.log(amount)// verwijdern als het werkt
})};


