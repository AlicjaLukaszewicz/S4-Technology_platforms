package org.example.repository;

import org.example.entity.Shop;
import org.example.entity.Tea;

import javax.persistence.EntityManager;
import javax.persistence.EntityManagerFactory;
import java.util.List;

public class TeaRepository extends JpaRepository<Tea, String> {

    /**
     * @param emf thread safe factory which will be used for creating {@link EntityManager}
     */
    public TeaRepository(EntityManagerFactory emf) {
        super(emf, Tea.class);
    }

    public List<Tea> findAllWithPriceLowerThan(long price) {
        EntityManager em = getEmf().createEntityManager();
        List<Tea> list = em.createQuery("select tea from Tea tea where tea.price < " + price, Tea.class).getResultList();
        em.close();
        return list;
    }

    public List<Tea> findAllFromShopCheaperThan(String shopName, long price) {
        EntityManager em = getEmf().createEntityManager();
        List<Tea> list = em.createQuery("select tea from Tea tea where tea.shop.name = :shopName and tea.price < :price", Tea.class).setParameter("shopName", shopName).setParameter("price", price).getResultList();
        em.close();
        return list;
    }

    public List<Tea> findAllFromShop(Shop shop) {
        EntityManager em = getEmf().createEntityManager();
        List<Tea> list = em.createQuery("select tea from Tea tea where tea.shop = :shop", Tea.class).setParameter("shop", shop).getResultList();
        em.close();
        return list;
    }
}