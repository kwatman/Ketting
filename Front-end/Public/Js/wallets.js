const myWallet =document.getElementById("myWallet")
const searchWalletBtn = document.getElementById("searchWalletButton")

searchWalletBtn.addEventListener("click", async(e) => {
    let localHost =localStorage.getItem("address") 
    const searchWalletInput = document.getElementById("searchWallet").value
    
    if(searchWalletInput.toString().length > 0){

        let walletKey ={
            "publicKey": searchWalletInput
        }

    await fetch("http://"+localHost+"/wallet",{
        method: "POST",
        headers: {"Content-Type":"application/json"},
        body: JSON.stringify(walletKey)   
    })
    .then(res => res.json())
    .then(res => { 
        const stringAdress = "Address: " + res.address;
        const stringBalance= "Balance: "+ res.balance;
        let table =document.getElementById("transactionTable");
        
        res.transactions.forEach((transaction) => {
            let transactionNumber = transaction.transactionNumber;
            let transactionAmount = transaction.amount;
            let transactionTimeStamp = transaction.timeStamp;
            let transactionSender = transaction.senderKey;
            let transactionReceiver = transaction.recieverKey;
            let transactionType = transaction.type;

            let row = table.insertRow(-1);
            
            let transactionTypeCel = row.insertCell(0);
            let transactionNumberCel = row.insertCell(1);
            let transactionAmountCel = row.insertCell(2);
            let transactionTimeStampCel = row.insertCell(3);
            let transactionSenderCel = row.insertCell(4);
            let transactionReceiverCel = row.insertCell(5);
            
            switch(transactionType){
                case 0:
                    transactionType = "Stake"
                    break;

                case 1:
                    transactionType = "Transaction"
                    break;
                case 2:
                    transactionType = "Reward"
                    break;
            }
            transactionTypeCel.innerHTML = transactionType;
            transactionNumberCel.innerHTML = transactionNumber;
            transactionAmountCel.innerHTML = transactionAmount;
            transactionTimeStampCel.innerHTML = transactionTimeStamp;
            transactionSenderCel.innerHTML = transactionSender.substring(0,20) + "...";
            transactionReceiverCel.innerHTML = transactionReceiver.substring(0,20) + "...";  
        })

        document.getElementById("wallet").innerHTML = '<li class="list-group-item text-truncate" >' + stringAdress + "</li>"+ '<li class="list-group-item text-truncate" >' + stringBalance + "</li></br></br>" ;
    });
    }
    else{
        alert("Login");
    }
    
});

myWallet.addEventListener("click", async(e) => {
    const loginPublic = localStorage.getItem("publicKey");
    let localHost =localStorage.getItem("address")
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
        let table =document.getElementById("transactionTable");
        

        res.transactions.forEach((transaction) => {
            let transactionNumber = transaction.transactionNumber;
            let transactionAmount = transaction.amount;
            let transactionTimeStamp = transaction.timeStamp;
            let transactionSender = transaction.senderKey;
            let transactionReceiver = transaction.recieverKey;
            let transactionType = transaction.type;

            let row = table.insertRow(-1);
            
            let transactionTypeCel = row.insertCell(0);
            let transactionNumberCel = row.insertCell(1);
            let transactionAmountCel = row.insertCell(2);
            let transactionTimeStampCel = row.insertCell(3);
            let transactionSenderCel = row.insertCell(4);
            let transactionReceiverCel = row.insertCell(5);
            
            switch(transactionType){
                case 0:
                    transactionType = "Stake"
                    break;

                case 1:
                    transactionType = "Transaction"
                    break;
                case 2:
                    transactionType = "Reward"
                    break;
            }
    
            transactionTypeCel.innerHTML = transactionType;
            transactionNumberCel.innerHTML = transactionNumber;
            transactionAmountCel.innerHTML = transactionAmount;
            transactionTimeStampCel.innerHTML = transactionTimeStamp;
            transactionSenderCel.innerHTML = transactionSender.substring(0,20) + "...";
            transactionReceiverCel.innerHTML = transactionReceiver.substring(0,20) + "...";  
        })

        document.getElementById("wallet").innerHTML = '<li class="list-group-item text-truncate" >' + stringAdress + "</li>"+ '<li class="list-group-item text-truncate" >' + stringBalance + "</li></br></br>" ;
    });
    }
    else{
        alert("Login");
    }
    
});