const localHostBtn = document.getElementById("localHostBtn");

let localHost = localStorage.getItem("address")

if(localHostBtn != null){
    localHostBtn.addEventListener("click", async(e) => {
        e.preventDefault();
        let localHostInput = document.getElementById("localHostInput").value; 
        
        if(localHostInput.toString().length == 0) {
            alert("Please enter ip or localhost address");
        }else{
            localStorage.setItem("address", localHostInput);
            localHost= localStorage.getItem("address");
        }  
})};











