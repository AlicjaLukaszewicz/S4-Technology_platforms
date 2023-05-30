package org.example;

import lombok.AllArgsConstructor;
import lombok.Getter;

@AllArgsConstructor
@Getter
public class Mage {
    private final String name;
    private final int level;

    @Override
    public String toString() {
        return "Mage{" + "name='" + name + '\'' + ", level=" + level + '}';
    }
}