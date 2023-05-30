package org.example;

import java.util.*;

public class Person implements Comparable<Person>, Comparator<Person> {
    private final String firstName;
    private final String lastName;
    private final int age;
    private final double income;
    private final Set<Person> subordinates;

    public Person() {
        this.firstName = "";
        this.lastName = "";
        this.age = 0;
        this.income = 0;
        this.subordinates = new HashSet<>();
    }

    public Person(String firstName, String lastName, int age, double income) {
        this.firstName = firstName;
        this.lastName = lastName;
        this.age = age;
        this.income = income;
        this.subordinates = new HashSet<>();
    }

    public static void printPersonHierarchy(Person person, int indent, Set<Person> visited) {
        if (person == null || visited.contains(person)) {
            return;
        }

        visited.add(person);
        System.out.println("-".repeat(indent) + person);

        for (Person subordinate : person.getSubordinates()) {
            printPersonHierarchy(subordinate, indent + 1, visited);
        }
    }

    public static Map<Person, Integer> getStatistics(Set<Person> personSet, String option) {
        Map<Person, Integer> result;
        if (Objects.equals(option, "natural")) {
            result = new TreeMap<>();
        } else if (Objects.equals(option, "alternative")) {
            result = new TreeMap<>(new Person());
        } else {
            result = new HashMap<>();
        }

        Set<Person> visited = new HashSet<>();
        for (Person employer : personSet) {
            int count = employer.getAllSubordinates().size() - 1;
            result.put(employer, count);
            for (Person employee : employer.getAllSubordinates()) {
                if (visited.contains(employee)) continue;
                visited.add(employee);
                count = employee.getAllSubordinates().size() - 1;
                result.put(employee, count);
            }
        }

        return result;
    }

    public void addSubordinate(Person o) {
        subordinates.add(o);
    }

    public Set<Person> getAllSubordinates() {
        Set<Person> allSubordinates = new HashSet<>();
        addSubordinatesToSet(this, allSubordinates);
        return allSubordinates;
    }

    private void addSubordinatesToSet(Person employee, Set<Person> allEmployees) {
        allEmployees.add(employee);
        for (Person subordinate : employee.getSubordinates()) {
            addSubordinatesToSet(subordinate, allEmployees);
        }
    }

    public Set<Person> getSubordinates() {
        return subordinates;
    }

    @Override
    public int compare(Person o1, Person o2) {
        if (o1.income != o2.income) return Double.compare(o1.income, o2.income);
        if (!Objects.equals(o1.firstName, o2.firstName)) return o1.firstName.compareTo(o2.firstName);
        if (!Objects.equals(o1.lastName, o2.lastName)) return o1.lastName.compareTo(o2.lastName);
        return Integer.compare(o1.age, o2.age);
    }

    @Override
    public int compareTo(Person o) {
        if (!Objects.equals(this.lastName, o.lastName)) return this.lastName.compareTo(o.lastName);
        if (!Objects.equals(this.firstName, o.firstName)) return this.firstName.compareTo(o.firstName);
        if (this.income != o.income) return Double.compare(this.income, o.income);
        return Integer.compare(this.age, o.age);
    }

    @Override
    public boolean equals(Object o) {
        if (o == this) return true;
        if (!(o instanceof Person other)) return false;
        boolean firstNameEquals = this.firstName.equals(other.firstName);
        boolean lastNameEquals = this.lastName.equals(other.lastName);
        boolean ageEquals = (this.age == other.age);
        boolean incomeEquals = (this.income == other.income);
        boolean subordinatesEquals = (this.subordinates == other.subordinates);

        return (firstNameEquals && lastNameEquals && ageEquals && incomeEquals && subordinatesEquals);
    }

    @Override
    public final int hashCode() {
        int result = 17;
        result = 31 * result + firstName.hashCode() + lastName.hashCode();
        return result;
    }

    @Override
    public String toString() {
        return this.firstName + " " + this.lastName + "[" + this.age + "]: " + this.income + " PLN";
    }
}
