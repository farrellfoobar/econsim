namespace EconSim.logic;

public abstract class Building
{
    protected Town hostTown;
    protected Building(Town hostTown) {
        this.hostTown = hostTown;
    }
    public abstract void doProductionTurn();
    public abstract void addWorker(int unemployedPopulation);
    public abstract void removeWorker(int amount);
}