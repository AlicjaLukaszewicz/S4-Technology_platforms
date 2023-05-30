package org.example;

import java.util.HashSet;
import java.util.Map;
import java.util.Set;
import java.util.TreeSet;

import static org.example.Person.getStatistics;
import static org.example.Person.printPersonHierarchy;

public class Main {
    public static void main(String[] args) {
        String sortingMethod = args[0];
        Set<Person> personSet = switch (sortingMethod) {
            case "natural" -> new TreeSet<>();
            case "alternative" -> new TreeSet<>(new Person());
            default -> new HashSet<>();
        };

        Person person1 = new Person("John", "Doe", 30, 5000);
        Person person2 = new Person("Jane", "Doe", 25, 4000);
        Person person3 = new Person("Alice", "Smith", 35, 6000);
        Person person4 = new Person("Bob", "Smith", 40, 7000);
        Person person5 = new Person("David", "Johnson", 45, 8000);
        Person person6 = new Person("Emily", "Johnson", 20, 3000);
        Person person7 = new Person("Frank", "Williams", 50, 9000);
        Person person8 = new Person("Grace", "Williams", 55, 10000);
        Person person9 = new Person("Henry", "Brown", 60, 11000);
        Person person10 = new Person("Isabella", "Brown", 18, 2000);

        person1.addSubordinate(person2);
        person1.addSubordinate(person3);
        person2.addSubordinate(person4);
        person2.addSubordinate(person5);
        person2.addSubordinate(person6);
        person4.addSubordinate(person7);
        person4.addSubordinate(person8);
        personSet.add(person1);
        personSet.add(person2);
        personSet.add(person4);
        personSet.add(person8);
        personSet.add(person9);
        personSet.add(person10);

        Set<Person> visited = new HashSet<>();
        for (Person person : personSet) {
            printPersonHierarchy(person, 0, visited);
        }

        Map<Person, Integer> result = getStatistics(personSet, sortingMethod);
        for (Object e : result.entrySet()) {
            System.out.println(((Map.Entry)e).getValue() + ": " +((Map.Entry)e).getKey());
        }
    }
}