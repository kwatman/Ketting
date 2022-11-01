import express from "express";

const port = 3000;
const app = express();

app.use(express.static('../Public'));
app.use(express.json());

//GET Requests
app.get("/blockchain" , (req, res) => {
    let blockchain = searchBlocks(
        req.query.prevHash,
        req.query.version,
        req.query.data,
        req.query.timestamp,
        req.query.publicKey,
        req.query.hash,
        req.query.signature);
});

app.get("/keypair" , (req, res)=>{
    let keypair = searchKeypair(
        req.query.privateKey,
        req.query.publicKey
    );
});

// app.get(`/wallet/${publicKey}/balance`, (req, res) => {

// });

// app.get(`/wallet/${publicKey}` , (req, res) =>{
//     let wallet = searchWallet(
//         req.query.address,
//         req.query.balance,
//         req.query.transaction
//     );
// });

//POST Requests
app.post("/transaction" , (req, res) => {

    let transaction = makeTransaction(
        req.query.transactionNumber,
        req.query.amount,
        req.query.timestamp,
        req.query.senderKey,
        req.query.receiverKey,
        req.query.signature
    );
    res.json({
        transaction: transaction
    });
});

app.listen(port, function() {
    console.log('listening on port ' + port);
});






