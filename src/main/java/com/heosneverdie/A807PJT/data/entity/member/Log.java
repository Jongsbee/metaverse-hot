package com.heosneverdie.A807PJT.data.entity.member;

import com.heosneverdie.A807PJT.common.BaseEntity;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Getter;
import lombok.NoArgsConstructor;

import javax.persistence.*;

@Table(name = "LOG")
@Entity
@Builder
@NoArgsConstructor
@AllArgsConstructor
@Getter
public class Log extends BaseEntity{
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;
    // member와 1:1 매핑

    @ManyToOne
    @JoinColumn(nullable = false, name = "member_id")
    private Member member;
    @Column(nullable = false, name = "event_code")
    private String eventCode;
    @Column(nullable = false)
    private Integer amount;
}
