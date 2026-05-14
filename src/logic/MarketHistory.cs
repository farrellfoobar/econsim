using System;
using System.Collections.Generic;
using System.Linq;

namespace EconSim.logic;

public class MarketHistory
{
    private const int supplyAndDemandMemoryLengthInTurns = TurnAndTimeManager.TurnsInAYear;
    private List<int> supplyByTurnModulo = new List<int>(supplyAndDemandMemoryLengthInTurns);
    private List<int> demandByTurnModulo = new List<int>(supplyAndDemandMemoryLengthInTurns);

    public MarketHistory() {
        for (int i = 0; i < supplyAndDemandMemoryLengthInTurns; i++) {
            supplyByTurnModulo.Add(0);
            demandByTurnModulo.Add(0);
        }
    }
    
    public int GetTotalDemand() {
        return demandByTurnModulo.Sum();
    }

    public int GetTotalSupply() {
        return supplyByTurnModulo.Sum();
    }

    public void AddSupply(int turnCount) {
        supplyByTurnModulo[turnCount % supplyAndDemandMemoryLengthInTurns]++;
    }

    public void AddDemand(int turnCount) {
        demandByTurnModulo[turnCount % supplyAndDemandMemoryLengthInTurns]++;
    }

    public void CullSupplyDemandHistory(int turnCount) {
        int turnCountToErase = Math.Abs(turnCount - supplyAndDemandMemoryLengthInTurns);
        int turnCountModuloToErase = turnCountToErase % supplyAndDemandMemoryLengthInTurns;

        supplyByTurnModulo[turnCountModuloToErase] = 0;
        demandByTurnModulo[turnCountModuloToErase] = 0;
    }
}