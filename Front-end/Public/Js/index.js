


let count = 0;
let localHost= "localhost:5262";

const loginBtn = document.getElementById("btnLogin");

const localHostBtn = document.getElementById("localHostBtn");

const completeTranscaction = document.getElementById("completeTranscaction")

const searchBlock = document.getElementById("searchBlockButton")

const myWallet =document.getElementById("myWallet")
const searchWalletBtn = document.getElementById("searchWalletButton")


if(localHostBtn != null){
    localHostBtn.addEventListener("click", async(e) => {
        e.preventDefault();
        let localHostInput = document.getElementById("localHostInput").value; 
        
        if(localHostInput.toString().length == 0) {
            alert("Please enter ip or localhost address");
        }else{
            localHost = document.getElementById("localHostInput").value; 
        }  
    })};

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

        await fetch("http://"+localHost+"/transaction",{
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
})};

if(searchWalletBtn != null){
searchWalletBtn.addEventListener("click", async(e) => {
        
    const searchWalletInput = document.getElementById("searchWalletInput").value
    
    if(searchWalletInput.toString().length > 0){

    await fetch("http://"+localHost+"/wallet",{
            method: "POST",
            headers: {"Content-Type":"application/json"},
            body: JSON.stringify(searchWalletInput)   
        })
        .then(res => res.json())
        .then(res => { 
            console.log(res)
            const stringAdress = "Address: " + res.address;
            const stringBalance= "Balance: "+ res.balance;
            document.getElementById("wallet").innerHTML = '<li class="list-group-item text-truncate" >' + stringAdress + "</li>"+ '<li class="list-group-item text-truncate" >' + stringBalance + "</li>"
        });
        }
        else{
            alert("Fill search field");
        }
    
})};

if(myWallet != null){
myWallet.addEventListener("click", async(e) => {
    const loginPublic = localStorage.getItem("publicKey");

    console.log(loginPublic);
    
    if(loginPublic != null){

        let publicKey ={
            "publicKey": loginPublic
        }

    await fetch("http://"+localHost+"/wallet",{
            method: "POST",
            headers: {"Content-Type":"application/json"},
            body: JSON.stringify(publicKey)   
        })
        .then(res => res.json())
        .then(res => { 
            const stringAdress = "Address: " + res.address;
            const stringBalance= "Balance: "+ res.balance;
            let lijstTransactions = "";
            

        res.transactions.forEach((transaction) => {
            let transactionNumber = transaction.transactionNumber;
            let transactionAmount = transaction.amount;
            let transactionTimeStamp = transaction.timeStamp;
            
            lijstTransactions += "Number: "+ transactionNumber + "|| Amount: " + transactionAmount + "|| Timestamp: "+ transactionTimeStamp + "</br>";
        })

            document.getElementById("wallet").innerHTML = '<li class="list-group-item text-truncate" >' + stringAdress + "</li>"+ '<li class="list-group-item text-truncate" >' + stringBalance + "</li>" + "<li class='list-group-item'>"+lijstTransactions +"</li>" ;
        });
        }
        else{
            alert("Login");
        }
        
})};



