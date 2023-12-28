namespace Direct4Me.Core.TravellingSalesmanProblem;

public class GeneticAlgorithm
{
    private readonly double _cr; // Crossover probability
    private readonly double _pm; // Mutation probability
    private readonly int _popSize;
    private readonly Random _random;
    private List<Tour>? _offspring;
    private List<Tour>? _population;

    public GeneticAlgorithm(int popSize, double cr, double pm)
    {
        _popSize = popSize;
        _cr = cr;
        _pm = pm;
        _random = new Random(1);
    }

    public Tour Execute(TspAlgorithm problem)
    {
        _population = new List<Tour>();
        _offspring = new List<Tour>();
        var best = new Tour(_popSize);

        for (var i = 0; i < _popSize; i++)
        {
            var newTour = problem.GenerateTour();
            problem.Evaluate(newTour);
            _population.Add(newTour);
            //TODO: Store the best tour
        }

        while (problem.GetNumberOfEvaluations() < problem.GetMaxEvaluations())
        {
            // Elitism - find the best and add to offspring using clone()
            while (_offspring.Count < _popSize)
            {
                var parent1 = TournamentSelection();
                var parent2 = TournamentSelection();
                //TODO: Ensure parents are not the same

                if (_random.NextDouble() < _cr)
                {
                    var children = Pmx(parent1, parent2);
                    _offspring.Add(children[0]);
                    if (_offspring.Count < _popSize) _offspring.Add(children[1]);
                }
                else
                {
                    _offspring.Add(parent1.Clone());
                    if (_offspring.Count < _popSize) _offspring.Add(parent2.Clone());
                }
            }

            foreach (var off in _offspring)
                if (_random.NextDouble() < _pm)
                    SwapMutation(off);

            //TODO: Evaluate population and store the best
            // This can be made more efficient by evaluating only those that have changed (mutated and crossed offspring)

            _population = new List<Tour>(_offspring);
            _offspring.Clear();
        }

        return best;
    }

    private void SwapMutation(Tour off)
    {
        // Perform mutation
    }

    private Tour[] Pmx(Tour parent1, Tour parent2)
    {
        // Perform PMX crossover to create two offspring
        return null;
    }

    private Tour TournamentSelection()
    {
        // Randomly select two DIFFERENT individuals and return the better one
        return null;
    }
}