// A simple templating method for replacing placeholders enclosed in curly braces.
if (!String.prototype.supplant) {
    String.prototype.supplant = function (o) {
        return this.replace(/{([^{}]*)}/g,
            function (a, b) {
                var r = o[b];
                return typeof r === 'string' || typeof r === 'number' ? r : a;
            }
        );
    };
}

$(function () {
    var notifier = $.connection.baconNotifier;
    var $baconTable = $('#baconTable');
    var $baconTableBody = $baconTable.find('tbody');

    function init() {
        notifier.server.getBaconStock().done(function(baconCollection) {
            $baconTableBody.empty();

            $.each(baconCollection, function() {
                var bacon = this;

                var row;
                if (bacon.BaconInStock)
                {
                    row = '<tr style="background-color: #11ff11;" id="' + bacon.BaconId + '">'
                }
                else
                {
                    row = '<tr style="background-color: #ff1111;" id="' + bacon.BaconId + '">'
                }

                row = row + '<td>' + bacon.BaconName + '</td><td>' + bacon.BaconStockQty + '</td></tr>'

                $baconTableBody.append(row);
            });
        });
    }

    notifier.client.updateBaconQty = function (bacon) {
        var row;
        if (bacon.BaconInStock) {
            row = '<tr style="background-color: #11ff11;" id="' + bacon.BaconId + '">'
        }
        else {
            row = '<tr style="background-color: #ff1111;" id="' + bacon.BaconId + '">'
        }

        row = row + '<td>' + bacon.BaconName + '</td><td>' + bacon.BaconStockQty + '</td></tr>'       

        $('#' + bacon.BaconId).replaceWith(row);
    }

    //uncomment line below to enable logging
    //$.connection.hub.logging = true;    

    $.connection.hub.start().done(init);
});