import express from "express";
import cors from "cors";

const port = 3000;
const app = express();

app.use(cors());
app.use(express.static('./Public'));
app.use(express.json());

app.listen(port, function() {
    console.log('listening on port ' + port);
});






