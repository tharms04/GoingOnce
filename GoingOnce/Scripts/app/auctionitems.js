var bValidBidder = false;
var bValidItem = false;

$(document).ready(function () {
    showAuctionItems();
    showBidders();

    $('#ItemNumber').focus();
});

$("#ItemNumber").focus(function () {
    $(this).select();
});

$('#ItemNumber').keyup(function () {
    showAuctionItems();
});

$('#PaddleNumber').keyup(function () {
    showBidders();
});

function showAuctionItems()
{
    bValidItem = false;
    $('#auction_items_table').hide();
    var selected_item = $("#ItemNumber").val();
    $(".auction_item").each(function () {
        if (this.id == "item_" + selected_item) {
            bValidItem = true;
            $('#auction_items_table').show();
            $(this).show();
        }
        else {
            $(this).hide();
        }
    });
    enableEnter();
}

function showBidders() {
    bValidBidder = false;
    $('#bidders_table').hide();
    var selected_bidder = $("#PaddleNumber").val();
    $(".bidder").each(function () {
        if (this.id == "bidder_" + selected_bidder) {
            bValidBidder = true;
            $('#bidders_table').show();
            $(this).show();
        }
        else {
            $(this).hide();
        }
    });
    enableEnter();
}

function enableEnter() {
    if (bValidBidder && bValidItem) 
    {
        $("#enter_bid_button").prop( "disabled", false );
    }
    else
    {
        $("#enter_bid_button").prop("disabled", true);
    }

}