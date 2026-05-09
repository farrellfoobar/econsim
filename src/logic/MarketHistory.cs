using System;
using System.Collections.Generic;
using System.Linq;

namespace EconSim.logic;

public class MarketHistory
{
    private const int SUPPLY_AND_DEMAND_MEMORY_LENGTH_IN_TURNS = 4;
    private int turnLastSeen = 0;
    private List<int> supplyByTurnModulo = new List<int>(SUPPLY_AND_DEMAND_MEMORY_LENGTH_IN_TURNS);
    private List<int> demandByTurnModulo = new List<int>(SUPPLY_AND_DEMAND_MEMORY_LENGTH_IN_TURNS);

    public MarketHistory() {
        for (int i = 0; i < SUPPLY_AND_DEMAND_MEMORY_LENGTH_IN_TURNS; i++) {
            supplyByTurnModulo.Add(0);
            demandByTurnModulo.Add(0);
        }
    }
    
    public int getTotalDemand() {
        return demandByTurnModulo.Sum();
    }

    public int getTotalSupply() {
        return supplyByTurnModulo.Sum();
    }

    public void addSupply(int turnCount) {
        supplyByTurnModulo[turnCount % SUPPLY_AND_DEMAND_MEMORY_LENGTH_IN_TURNS]++;
    }

    public void addDemand(int turnCount) {
        demandByTurnModulo[turnCount % SUPPLY_AND_DEMAND_MEMORY_LENGTH_IN_TURNS]++;
    }

    public void cullSupplyDemandHistory(int turnCount) {
        int turnCountToErase = Math.Abs(turnCount - SUPPLY_AND_DEMAND_MEMORY_LENGTH_IN_TURNS);
        int turnCountModuloToErase = turnCountToErase % SUPPLY_AND_DEMAND_MEMORY_LENGTH_IN_TURNS;

        supplyByTurnModulo[turnCountModuloToErase] = 0;
        demandByTurnModulo[turnCountModuloToErase] = 0;
    }
}