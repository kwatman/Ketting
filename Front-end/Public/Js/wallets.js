const myWallet =document.getElementById("myWallet")
const searchWalletBtn = document.getElementById("searchWalletButton")
let localHost =localStorage.getItem("address")

searchWalletBtn.addEventListener("click", async(e) => {
        
    const searchWalletInput = document.getElementById("searchWalletInput").value
    
    if(searchWalletInput.toString().length > 0){

    await fetch("http://"+localHost+"/wallet",{
        method: "POST",
        headers: {"Content-Type":"application/json"},
        body: JSON.stringify(searchWalletInput)   
    })
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
    }
    else{
        alert("Login");
    }
    
});

myWallet.addEventListener("click", async(e) => {
    const loginPublic = localStorage.getItem("publicKey");
    
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
    
});