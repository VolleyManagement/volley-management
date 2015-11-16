var counter = 1;

function addInput(divName, startIndex, limit, alertMsg) {
    if (counter == limit) {
        alert(alertMsg);
    }
    else {
        var newdiv = document.createElement('div');
        newdiv.innerHTML = "<br><input type='text' name='Divisions[" + (counter + startIndex) + "].Name'>";
        document.getElementById(divName).appendChild(newdiv);
        counter++;
    }
}