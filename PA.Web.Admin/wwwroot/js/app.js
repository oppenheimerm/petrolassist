

async function initMap(apiKey) {
    //  Setup html doc
    const parentElement = document.getElementById(`mapHolder`); // a <div>
    const script = document.createElement(`script`);
    script.src = `https://maps.googleapis.com/maps/api/js?key=${apiKey}`;
    script.async = true;
    script.defer = true;
    parentElement.insertBefore(script, null);
    script.onload = async function () { }
    
}

// https://gabrieleromanato.name/javascript-how-to-use-the-google-maps-api-with-promises-and-async-await
function getCoordinates(address) {

    return new Promise((resolve, reject) => {
        const geocoder = new google.maps.Geocoder();
        geocoder.geocode({ address: address }, (results, status) => {
            if (status === 'OK') {
                console.log("Latitude: " + results[0].geometry.location.lat())
                console.log("Longitude: " + results[0].geometry.location.lng())

                document.getElementById('station-lat').value = results[0].geometry.location.lat();
                document.getElementById('station-long').value = results[0].geometry.location.lng();

                var results = {
                    "latitude": results[0].geometry.location.lat(), "longitude": results[0].geometry.location.lng()
                };

                resolve(results);
                
            } {
                //reject(status);
            }
        });
    });
}



//https://stackoverflow.com/questions/1398657/dynamically-loading-google-maps-apis
/*async function getCoordinates(address) {

    var geocoder = new google.maps.Geocoder();
    
    geocoder.geocode({ 'address': address }, function (results, status) {

        //https://developers.google.com/maps/documentation/javascript/geocoding
        if (status == 'OK') {
            console.log("Latitude: " + results[0].geometry.location.lat())
            console.log("Longitude: " + results[0].geometry.location.lng())
            
            document.getElementById('station-lat').value = results[0].geometry.location.lat();
            document.getElementById('station-long').value = results[0].geometry.location.lng();

            var results = {
                "coordinates": [
                    { "latitude": results[0].geometry.location.lat(), "longitude": results[0].geometry.location.lng() }
                ]
            };

            return results
        }
        //  Make Lat / Long input empty not 0! to prevent saving
        //  without valid lat/long details
        else { }
    }); 
 
}*/

function mapsScriptSet() {
    //  document.getElementById('textbox_id').value
    var isSet = document.getElementById('mapScripLoaded').value
    return isSet == "true" ? true : false
}

function setMapScriptStatus(val) {
    var isSet = document.getElementById('mapScripLoaded').value = val;
}