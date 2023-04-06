package com.heosneverdie.A807PJT.data.entity.member;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Getter;
import lombok.NoArgsConstructor;

import javax.persistence.*;

@Table(name = "Account")
@Entity
@Builder
@NoArgsConstructor
@AllArgsConstructor
@Getter
public class Account {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;
    // member와 1:1 매핑

    @OneToOne
    @JoinColumn(nullable = false, name = "member_id")
    private Member member;
    @Column(nullable = false)
    private int exp;
    @Column(nullable = false)
    private int coin;

    public void updateExpAndCoin(int exp, int coin) {
        this.exp += exp;
        this.coin += coin;
    }
}
