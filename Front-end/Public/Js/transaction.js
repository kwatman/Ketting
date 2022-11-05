const completeTranscaction = document.getElementById("completeTranscaction")
let localHost =localStorage.getItem("address")

completeTranscaction.addEventListener("click", async(e)=>{

    e.preventDefault();
    const stakeRad = document.getElementById("radStake");
    const transactionRad = document.getElementById("radTransaction");
    const amount= document.getElementById("amount").value;
    const receiver = document.getElementById("receiver").value;
    const loginPrivate = localStorage.getItem("privateKey");
    const loginPublic = localStorage.getItem("publicKey");
    let date = new Date().toLocaleDateString("fr-CA",{  year: 'numeric', month: '2-digit', day: '2-digit' }) + "T" + new Date().toLocaleTimeString("nl-BE");
    let type = 0;

    if(stakeRad.checked){
        type = 0;
    } 
    else if(transactionRad.checked){
        type = 1;
    }else{
        alert("Please select if stake or transaction")
    }
    


    if(amount.toString().length > 0 && receiver.toString().length > 0){
        
        let publicKey = {
            "publicKey": loginPublic
        };

    let count = 0; 
    await fetch("http://localhost:5262/wallet",{
        method: "POST",
        headers: {"Content-Type":"application/json"},
        body: JSON.stringify(publicKey),
        })
        .then(res => res.json())
        .then((res) => { 
        
        res.transactions.forEach((transaction) => {
            if(transaction.type == 1){
                count += 1;
            }        
        }) 
    });

    let signNotEncrypt = count + "@" + loginPublic + "@" + receiver  + "@" + amount + "@" + date;
    var sign = new JSEncrypt();
    sign.setPrivateKey(loginPrivate);
    var signature = sign.sign(signNotEncrypt, CryptoJS.SHA256, "sha256");

    let transaction = {
        "transactionNumber": count++,
        "amount": amount,
        "timeStamp": date,
        "senderKey": loginPublic,
        "recieverKey": receiver,
        "signature": signature,
        "type": type
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
});