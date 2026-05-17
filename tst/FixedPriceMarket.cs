using System.Collections.Generic;
using EconSim.data;
using EconSim.logic;

namespace EconSim.tst;

public class FixedPriceMarket : Market
{
    private Dictionary<ItemType, CoinAmount> forcedPrice;
    public FixedPriceMarket(TurnAndTimeManager turnManager) : base(turnManager)
    {
        forcedPrice = new Dictionary<ItemType, CoinAmount>();
    }

    public override CoinAmount GetPrice(ItemType itemType) {
        return forcedPrice.ContainsKey(itemType) ? forcedPrice[itemType] : SimulationConstants.BasePrice[itemType];
    }

    public void SetPrice(ItemType item, CoinAmount price)
    {
        forcedPrice[item] = price;
    }
}