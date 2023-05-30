package org.example;

import java.util.Collection;
import java.util.Optional;

public class MageRepository {
    private final Collection<Mage> collection;

    public MageRepository(Collection<Mage> collection) {
        this.collection = collection;
    }

    public void save(Mage mage) throws IllegalArgumentException {
        if (this.collection.contains(mage)) {
            throw new IllegalArgumentException("Mage '" + mage.getName() + "' does already exist.");
        } else {
            this.collection.add(mage);
        }
    }

    public void delete(String name) throws IllegalArgumentException {
        Mage mage = collection.stream().filter(currentMage -> currentMage.getName().equals(name)).findFirst().orElseThrow(() -> new IllegalArgumentException("Mage '" + name + "' does not exist."));
        collection.remove(mage);
    }

    public Optional<Mage> find(String name) {
        return collection.stream().filter(mage -> mage.getName().equals(name)).findFirst();
    }
}