using EconSim.data;
using EconSim.logic;

namespace EconSim.tst;

public class FixedPriceMarket : Market
{
    public FixedPriceMarket(TurnAndTimeManager turnManager) : base(turnManager) {}

    public override CoinAmount getPrice(ItemType itemType) {
        return SimulationConstants.BASE_PRICE[itemType];
    }
}