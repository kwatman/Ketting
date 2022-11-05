const loginBtn = document.getElementById("btnLogin");

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
});