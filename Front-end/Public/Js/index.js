import Wallet from "./wallet.js"; 
import Block from "./block.js";
//import WalletInfo from "./walletInfo.js";


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
        const loginPrivate = localStorage.getItem("privateKey");
        const loginPublic = localStorage.getItem("publicKey");
        let date = new Date().toLocaleDateString("fr-CA",{  year: 'numeric', month: '2-digit', day: '2-digit' }) + "T" + new Date().toLocaleTimeString("nl-BE");
        
        let signNotEncrypt = count + "@" + loginPublic + "@" + receiver  + "@" + amount + "@" + date;
        console.log(signNotEncrypt);

        if(amount.toString().length > 0 && receiver.toString().length > 0){
            
        var sign = new JSEncrypt();
        sign.setPrivateKey(loginPrivate);
        var signature = sign.sign(signNotEncrypt, CryptoJS.SHA256, "sha256");
        
        console.log(signNotEncrypt);
        console.log(signature);
        let transaction = {
            "transactionNumber": count,
            "amount": amount,
            "timeStamp": date,
            "senderKey": loginPublic,
            "recieverKey": receiver,
            "signature": signature,
            "type": 0
        }
        console.log(transaction);

        await fetch("http://localhost:2400/transaction",{
            method: "POST",
            headers: {"Content-Type":"application/json"},
            body: JSON.stringify(transaction)   
        }).then(async(res) => { 
                alert("Transaction of " + amount + " has been completed");
        });

        }
        else{
            alert("Fill the inputfields to make transaction");
        }
    
        console.log(amount)// verwijdern als het werkt
})};

// fetch(`http://localhost:7262/wallet/${publicKey}`)
// .then((response) => {
//     return response.json();
// })
// .then((wallets) => {
//     wallets = wallets.map((wallet)=> {
//         return new Wallet(
//             wallet._balance,
//             wallet.     
//     });
//     const walletsSection = document.getElementById("produkten");
//     let produktenHtmlString ="";
//     produkten.forEach((produkt, index)=>{
//         if(index % 4 == 0) produktenHtmlString += "<div class='row'>" 
//         produktenHtmlString += produkt.toHtmlString();
//         if(index % 4 == 3 || index === produkten.length - 1) produktenHtmlString += "</div>"
//     });
//     produktenSection.innerHTML = produktenHtmlString;
    
//     const bestelButtons = document.querySelectorAll(".toevoegen");
//     for (var i = 0; i < bestelButtons.length; i++){
//         const bestelButton = bestelButtons[i];
//         bestelButton.addEventListener("click",(e) =>{
//         if(bestelling == null){
//             bestelling = new Bestelling();
//         }
//         let id = `${e.target.id}`
        
//         bestelling.addBesteldProdukt(id);
//         bestellingTableBody.innerHTML = bestelling.toHtmlString();

    
//         });
//     }

// });



