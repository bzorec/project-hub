namespace Direct4Me.Core.TravellingSalesmanProblem;

public class GeneticAlgorithm
{
    private readonly double _crossoverProbability;
    private readonly double _mutationProbability;
    private readonly int _popSize;
    private readonly Random _random;
    private List<Tour>? _offspring;
    private List<Tour>? _population;

    public GeneticAlgorithm(int popSize, double crossoverProbability, double mutationProbability)
    {
        _popSize = popSize;
        _crossoverProbability = crossoverProbability;
        _mutationProbability = mutationProbability;
        _random = new Random(1);
    }

    public Tour? Execute(TspAlgorithm problem)
    {
        _population = new List<Tour>();
        _offspring = new List<Tour>();
        Tour? best = null;

        for (var i = 0; i < _popSize; i++)
        {
            var newTour = problem.GenerateTour();
            problem.Evaluate(newTour);
            _population.Add(newTour);
            if (best == null || newTour.Distance < best.Distance)
                best = newTour.Clone();
        }

        while (problem.GetNumberOfEvaluations() < problem.GetMaxEvaluations())
        {
            if (best != null) _offspring.Add(best.Clone() ?? throw new InvalidOperationException());

            while (_offspring.Count < _popSize)
            {
                var parent1 = TournamentSelection();
                var parent2 = TournamentSelection();
                while (parent1 == parent2) parent2 = TournamentSelection();

                if (_random.NextDouble() < _crossoverProbability)
                {
                    var children = Pmx(parent1, parent2);
                    _offspring.Add(children[0]);
                    if (_offspring.Count < _popSize) _offspring.Add(children[1]);
                }
                else
                {
                    _offspring.Add(parent1.Clone() ?? throw new InvalidOperationException());
                    if (_offspring.Count < _popSize)
                        _offspring.Add(parent2.Clone() ?? throw new InvalidOperationException());
                }
            }

            foreach (var offspring in _offspring)
            {
                if (_random.NextDouble() < _mutationProbability) SwapMutation(offspring);
                problem.Evaluate(offspring);
                if (best == null || offspring.Distance < best.Distance) best = offspring.Clone();
            }

            _population = new List<Tour>(_offspring);
            _offspring.Clear();
        }

        return best ?? throw new InvalidOperationException("No solution found.");
    }

    private void SwapMutation(Tour tour)
    {
        var index1 = _random.Next(tour.Path.Count);
        var index2 = _random.Next(tour.Path.Count);

        while (index1 == index2) index2 = _random.Next(tour.Path.Count);

        (tour.Path[index1], tour.Path[index2]) = (tour.Path[index2], tour.Path[index1]);
    }

    private Tour[] Pmx(Tour parent1, Tour parent2)
    {
        var cutPoint1 = _random.Next(parent1.Path.Count);
        var cutPoint2 = _random.Next(parent1.Path.Count);

        if (cutPoint1 > cutPoint2) (cutPoint1, cutPoint2) = (cutPoint2, cutPoint1); // Swap

        var child1 = new Tour(parent1.Dimension);
        var child2 = new Tour(parent2.Dimension);

        for (var i = cutPoint1; i <= cutPoint2; i++)
        {
            child1.Path[i] = parent2.Path[i];
            child2.Path[i] = parent1.Path[i];
        }

        return new[] { child1, child2 };
    }

    private Tour TournamentSelection()
    {
        if (_population == null) throw new Exception("Population is null");

        var index1 = _random.Next(_population.Count);
        var index2 = _random.Next(_population.Count);

        while (index1 == index2) index2 = _random.Next(_population.Count);

        var tour1 = _population[index1];
        var tour2 = _population[index2];

        return tour1.Distance < tour2.Distance ? tour1 : tour2;
    }
}